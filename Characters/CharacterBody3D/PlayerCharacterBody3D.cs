using Godot;
using System;

namespace ThreeDLib
{
    public partial class PlayerCharacterBody3D : CharacterBody3D
    {
        [Export]
        public bool isControlled = false;
        [Export]
        public bool isOnHoverBike = false;

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

        // Most recent HoverBike object that entered the interactable area
        public HoverBike mostRecentHoverBike;

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
            if (isOnHoverBike) 
                return State.InVehicle;
            
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
                if (mostRecentHoverBike != null && !isOnHoverBike)
                {
                    isControlled = false;
                    isOnHoverBike = true;
                    parent.RemoveChild(this);
                    mostRecentHoverBike.AddChild(this);
                    GlobalPosition = mostRecentHoverBike.GlobalPosition;
                    GlobalRotationDegrees = mostRecentHoverBike.GlobalRotationDegrees;
                    GlobalRotationDegrees = GlobalRotationDegrees with { Y = GlobalRotationDegrees.Y + 180 };
                    Velocity = Vector3.Zero;
                    mostRecentHoverBike.isControlled = true;
                }
                else if (isOnHoverBike)
                {
                    var position = mostRecentHoverBike.GlobalPosition;
                    mostRecentHoverBike.isControlled = false;
                    isControlled = true;
                    isOnHoverBike = false;
                    mostRecentHoverBike.RemoveChild(this);
                    parent.AddChild(this);
                    GlobalPosition = position with { Y = GlobalPosition.Y + 2 };
                }
            }
        }

        public void OnInteractionAreaAreaEntered(Area3D area)
        {
            if (!isOnHoverBike)
            {
                if (area.IsInGroup("Vehicle"))
                {
                    var parent = area.GetParent<RigidBody3D>();
                    mostRecentHoverBike = (HoverBike) parent;
                    GetNode<GameEventHandler>("/root/GameEventHandler").PlayerInRangeOfInteractableObject(parent);
                }
            }
        }

        public void OnInteractionAreaAreaExited(Area3D area)
        {
            GetNode<GameEventHandler>("/root/GameEventHandler").PlayerOutOfRangeOfInteractableObject(parent);
        }

        public void OnInteractionAreaBodyEntered(Node3D body)
        {
            if (!isOnHoverBike)
            {
                if (body.IsInGroup("Vehicle"))
                {
                    if (body is HoverBike)
                        mostRecentHoverBike = (HoverBike) body;
                }
            }
        }

        public void OnInteractionAreaBodyExited(Node3D body)
        {
            if (!isOnHoverBike)
            {
                if (body.IsInGroup("Vehicle"))
                {
                    // mostRecentVehicle = null;
                }
            }
        }
    }
}