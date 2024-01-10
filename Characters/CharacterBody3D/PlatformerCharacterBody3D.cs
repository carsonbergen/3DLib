using Godot;
using System;

namespace ThreeDLib
{
    public partial class PlatformerCharacterBody3D : PlayerCharacterBody3D
    {
        // Platformer logic related variables
        private bool doubleJumpAvailable = true;
        private State currentState;
        private float movementFactor = 1f;
        private Vector3 jumpDirection;

        public override void _PhysicsProcess(double delta)
        {
            currentState = GetState();
            if (isControlled && currentState != State.InVehicle)
                Velocity = CalculateMovement(delta);
            HandleInteraction(delta);
            MoveAndSlide();
        }

        public override void _Process(double delta)
        {
            GD.Print(health);
            if (currentState == State.InVehicle)
            {
                if (mostRecentHoverBike != null)
                {
                    GlobalPosition = mostRecentHoverBike.seat.GlobalPosition;
                    GlobalRotationDegrees = mostRecentHoverBike.seat.GlobalRotationDegrees with
                    {
                        X = -mostRecentHoverBike.seat.GlobalRotationDegrees.X,
                        Y = mostRecentHoverBike.seat.GlobalRotationDegrees.Y + 180,
                        Z = -mostRecentHoverBike.seat.GlobalRotationDegrees.Z
                    };
                }
            }
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
            if (inputDir != Vector2.Zero && currentState != State.InAir)
            {
                GlobalRotationDegrees = GlobalRotationDegrees with { Y = camera.GlobalRotationDegrees.Y };
                model.GlobalRotationDegrees = model.GlobalRotationDegrees with
                {
                    Y = (float)Mathf.Lerp(model.GlobalRotationDegrees.Y, camera.GlobalRotationDegrees.Y, delta * 10)
                };
            }
            
            Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

            // Double jump
            if (Input.IsActionJustPressed("move_jump") && currentState == State.InAir && doubleJumpAvailable)
            {
                velocity.Y = jumpVelocity;
                doubleJumpAvailable = false;
                jumpDirection = direction;
            }
            else if (Input.IsActionJustPressed("move_jump") && currentState != State.InAir)
            {
                velocity.Y = jumpVelocity;
                jumpDirection = direction;
            }
            else if (currentState != State.InAir)
            {
                jumpDirection = Vector3.Inf;
            }

            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * speed;
                velocity.Z = direction.Z * speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
                velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
            }

            return velocity;
        }
    }
}