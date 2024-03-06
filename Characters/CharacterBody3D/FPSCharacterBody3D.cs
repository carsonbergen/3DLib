using Godot;
using System;

namespace ThreeDLib
{
    public partial class FPSCharacterBody3D : PlayerCharacterBody3D
    {
        // TODO: load from config file
        [Export]
        public float mouseSensitivity = 0.5f;

        // Holds the camera
        [Export]
        public Node3D upperBody = null;

        [Export]
        public float jumpDistance = 5f;

        private float movementFactor = 1f;
        private Vector3 jumpDirection;

        public override void _Ready()
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        public override void _PhysicsProcess(double delta)
        {
            currentState = GetState();
            GD.Print(currentState);
            if (isControlled && currentState != State.InVehicle)
                Velocity = CalculateMovement(delta);
            MoveAndSlide();
        }

        public override void _Input(InputEvent @event)
        {
            if ((@event is InputEventMouseMotion eventMouseMotion) && (Input.MouseMode == Input.MouseModeEnum.Captured))
            {

                RotateY(Mathf.DegToRad(-eventMouseMotion.Relative.X * mouseSensitivity));
                upperBody.RotateX(Mathf.DegToRad(-eventMouseMotion.Relative.Y * mouseSensitivity));
                upperBody.RotationDegrees = upperBody.RotationDegrees with { X = Mathf.Clamp(upperBody.RotationDegrees.X, -90, 89) };
            }
            if (@event is InputEventKey inputEventKey) {
                if (inputEventKey.IsActionPressed("open_menu")) {
                    if (Input.MouseMode == Input.MouseModeEnum.Captured)
                        Input.MouseMode = Input.MouseModeEnum.Visible;
                    else 
                        Input.MouseMode = Input.MouseModeEnum.Captured;
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
                movementFactor = 1f;
            }

            Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward") * movementFactor;

            Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

            if (Input.IsActionJustPressed("move_jump") && currentState != State.InAir)
            {
                velocity.Y = jumpVelocity;
                jumpDirection = direction;
            }
            else if (currentState != State.InAir)
            {
                jumpDirection = Vector3.Zero;
            }
            
            if (jumpDirection != Vector3.Zero) {
                velocity.X = Mathf.MoveToward(Velocity.X, jumpDirection.X * jumpDistance, speed);
                velocity.Z = Mathf.MoveToward(Velocity.Z, jumpDirection.Z * jumpDistance, speed);
            }
            else if (direction != Vector3.Zero)
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
