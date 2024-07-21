using Godot;
using System;
using ThreeDLib;

namespace FPS
{
    [GlobalClass]
    public partial class PlayerAttributes : Resource
    {
        [Export]
        public float health { get; set; }

        [Export]
        public float maxHealth { get; set; }

        [Export]
        public Weapon[] weapons { get; set; }

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