using System;
using Godot;

namespace ThreeDLib
{
    public partial class Weapon : Node3D
    {
        [Export]
        public Marker3D leftArmTarget;
        [Export]
        public Marker3D rightArmTarget;

        [Export]
        public float damage = 0f;
        [Export]
        public string name = "";
    }
}