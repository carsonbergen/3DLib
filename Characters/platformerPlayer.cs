using Godot;
using System;

namespace ThreeDLib
{
    public partial class platformerPlayer : player
    {
        public const float Speed = 5.0f;
        public const float JumpVelocity = 4.5f;

        public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

        private bool doubleJumpAvailable = true;

        private State currentState;

        private float movementFactor = 1f;

        private Vector3 jumpDirection;

        public override void _PhysicsProcess(double delta)
        {
            currentState = GetState();
            Velocity = CalculateMovement(delta);
            MoveAndSlide();
        }

        public override Vector3 CalculateMovement(double delta)
        {
            Vector3 velocity = Velocity;

            if (currentState == State.InAir)
            {
                velocity.Y -= gravity * (float)delta;
                movementFactor = 0.25f;
            }
            else if (currentState != State.InAir)
            {
                doubleJumpAvailable = true;
                movementFactor = 1f;
            }

            Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward") * movementFactor;
            /**
                TODO:
                    Add lock on, and turning off lock on
            */
            if (inputDir != Vector2.Zero)
            {
                RotationDegrees = RotationDegrees with {Y = camera.RotationDegrees.Y};
            }
            Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

            // Double jump
            if (Input.IsActionJustPressed("move_jump") && currentState == State.InAir && doubleJumpAvailable)
            {
                velocity.Y = JumpVelocity;
                doubleJumpAvailable = false;
                jumpDirection = direction;
            }
            else if (Input.IsActionJustPressed("move_jump") && currentState != State.InAir)
            {
                velocity.Y = JumpVelocity;
                jumpDirection = direction;
            }
            else if (currentState != State.InAir)
            {
                jumpDirection = Vector3.Inf;
            }

            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * Speed;
                velocity.Z = direction.Z * Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
                velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
            }

            return velocity;
        }
    }
}