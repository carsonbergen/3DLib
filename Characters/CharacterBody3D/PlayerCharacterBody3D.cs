using Godot;
using System;

namespace ThreeDLib
{
    public partial class PlayerCharacterBody3D : CharacterBody3D
    {
        [Export]
        public bool isControlled = false;
        [Export]
        public bool isInVehicle = false;

        [ExportCategory("Physics")]
        [Export]
        public const float speed = 5.0f;
        [Export]
        public const float jumpVelocity = 4.5f;
        [Export]
        public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

        [ExportCategory("Internal nodes")]
        [Export]
        public Area3D interactionArea;
        [Export]
        public Node3D model;
        [ExportCategory("External nodes")]
        [Export]
        public Node3D camera;

        private Node3D parent;

        // Most recent object that entered the interactable area
        private Node3D mostRecentVehicle;

        public override void _Ready()
        {
            if (camera == null)
            {
                GD.PrintErr("Camera can't be null");
                GetTree().Quit();
            }
            // Gets the root node3d of the current scene (not the actual root)
            parent = GetNode<Node3D>("../");
        }

        /**
            Gets the current state of the player object based on physical state.
        */
        public State GetState()
        {
            if (!IsOnFloor())
            {
                return State.InAir;
            }

            if (Velocity != Vector3.Zero)
            {
                if (Input.IsActionPressed("move_sprint"))
                {
                    return State.Sprinting;
                }
                return State.Walking;
            }

            return State.Idle;
        }

        /**
            Method meant to be overridden by a player subclass.
        */
        public virtual Vector3 CalculateMovement(double delta) { return Vector3.Zero; }

        /**
            Method for handling interaction
        */
        public virtual void HandleInteraction(double delta)
        {
            if (Input.IsActionJustPressed("interact"))
            {
                if (mostRecentVehicle != null && !isInVehicle)
                {
                    isControlled = false;
                    isInVehicle = true;
                    parent.RemoveChild(this);
                    mostRecentVehicle.AddChild(this);
                    GlobalPosition = mostRecentVehicle.GlobalPosition;
                    GlobalRotationDegrees = mostRecentVehicle.GlobalRotationDegrees;
                    GlobalRotationDegrees = GlobalRotationDegrees with { Y = GlobalRotationDegrees.Y + 180 };
                    Velocity = Vector3.Zero;
                }
                else if (isInVehicle)
                {
                    var position = mostRecentVehicle.GlobalPosition;
                    isControlled = true;
                    isInVehicle = false;
                    mostRecentVehicle.RemoveChild(this);
                    parent.AddChild(this);
                    GlobalPosition = position with { Y = GlobalPosition.Y + 2 };
                }
            }
        }

        public void OnInteractionAreaBodyEntered(Node3D body)
        {
            if (!isInVehicle)
            {
                if (body.IsInGroup("Vehicle"))
                {
                    mostRecentVehicle = body;
                }
            }
        }

        public void OnInteractionAreaBodyExited(Node3D body)
        {
            if (!isInVehicle)
            {
                if (body.IsInGroup("Vehicle"))
                {
                    // mostRecentVehicle = null;
                }
            }
        }
    }
}