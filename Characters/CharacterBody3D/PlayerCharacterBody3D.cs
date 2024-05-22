using Godot;
using Godot.Collections;

/**
    
*/
namespace ThreeDLib
{
    public partial class PlayerCharacterBody3D : CharacterBody3D
    {
        [Export]
        public bool isControlled = false;
        [Export]
        public bool isInVehicle = false;
        [Export]
        public bool canBeDamaged = true;
        [Export]
        public float health = 10f;

        [ExportCategory("Physics")]
        [Export]
        public float walkSpeed = 5.0f;
        [Export]
        public float sprintSpeed = 10f;
        [Export]
        public float jumpVelocity = 4.5f;
        [Export]
        public float speedChangeFactor = 10f;
        [Export]
        public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

        // All of the nodes to be set when modifying a player scene
        [ExportCategory("Internal nodes")]
        [Export]
        public Area3D interactionArea;
        [Export]
        public Node3D model;

        // All of the nodes to be set outside of the player
        [ExportCategory("External nodes")]
        [Export]
        public Node3D camera;

        private Node3D parent;

        // Needs to have a default value greater than zero as otherwise
        // getState will not change state into walking or sprinting.
        public float speed = 0;

        // Use a queue instead
        public Godot.Collections.Dictionary damagers = new();

        public State currentState;

        public void Setup()
        {
            speed = walkSpeed;
            currentState = State.Idle;
            if (camera == null)
            {
                GD.PrintErr("Camera can't be null");
                GetTree().Quit();
            }
            // Gets the root node3d of the current scene (not the actual root)
            parent = GetNode<Node3D>("../");
        }

        /**
            Gets the current state of the player object based on physical state in the Godot world.
        */
        public State GetState()
        {
            if (isInVehicle)
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
            Calculates Velocity vector for use in _PhysicsProcess
        */
        public virtual Vector3 CalculateMovement(double delta) { return Vector3.Zero; }
    }
}