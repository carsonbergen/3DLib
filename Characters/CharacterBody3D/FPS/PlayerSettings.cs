using Godot;
using System;

namespace FPS
{
    [GlobalClass]
    public partial class PlayerSettings : Resource
    {
        [Export]
        public float lookSensitivity = 0.5f;
        [Export]
        public float adsSensitivity = 0.25f;
        [Export]
        public int fov = 110;

        [Export]
        public float jumpDistance = 1f;
        [Export]
        public float walkSpeed = 5f;
        [Export]
        public float sprintSpeed = 10f;
        [Export]
        public float adsWalkSpeed = 2.5f;
        [Export]
        public float crouchSpeed = 3f;
        [Export]
        public float jumpVelocity = 3f;
        [Export]
        public float speedChangeFactor = 10f;
        [Export]
        public float inAirMovementFactor = 0.5f;
        [Export]
        public float gravity = 9.81f;
    }
}