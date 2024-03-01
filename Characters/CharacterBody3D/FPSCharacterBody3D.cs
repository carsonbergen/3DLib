using Godot;
using System;

namespace ThreeDLib
{
    public partial class FPSCharacterBody3D : PlayerCharacterBody3D
    {
        [Export]
        public float mouseSensitivity = 0.5f;
        [Export]
        public Node3D upperBody = null;
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion eventMouseMotion)
            {
                RotateY(Mathf.DegToRad(-eventMouseMotion.Relative.X * mouseSensitivity));
                upperBody.RotateX(Mathf.DegToRad(-eventMouseMotion.Relative.Y * mouseSensitivity));
                upperBody.RotationDegrees = upperBody.RotationDegrees with { X = Mathf.Clamp(upperBody.RotationDegrees.X, -90, 89) };
            }
        }
    }
}
