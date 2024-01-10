using System;
using Godot;

namespace ThreeDLib
{
    public partial class Weapon : Area3D
    {
        [Export]
        public float damage = 0f;
    }
}