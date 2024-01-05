using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

namespace ThreeDLib
{
    public partial class PlatformerRigidBody3D : PlayerRigidBody3D
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
            // currentState = GetState();
            ApplyCentralForce(CalculateMovement(delta));
        }

        public override Vector3 CalculateMovement(double delta)
        {
            Vector3 input = Vector3.Zero;
            input.X = Input.GetAxis("move_left", "move_right");
            input.Z = Input.GetAxis("move_forward", "move_backward");

            var force = input * 12000f * (float) delta;

            GD.Print("force: ", force);
            return force;
        }
    }
}