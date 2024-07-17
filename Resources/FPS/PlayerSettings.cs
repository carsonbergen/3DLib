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
    }
}