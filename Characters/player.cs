using Godot;
using System;

namespace ThreeDLib
{
    public partial class player : CharacterBody3D
    {
        [ExportCategory("External nodes")]
        [Export]
        public Node3D camera;

        public override void _Ready()
        {
            if (camera == null)
            {
                GD.PrintErr("Camera can't be null");
                GetTree().Quit();
            }
        }

        /**
            Gets the current state of the player object based on physical state.
        */
        public State GetState() 
        {
            if (!IsOnFloor()) 
            {
                return State.InAir;
            }

            if (Velocity != Vector3.Zero)
            {
                if (Input.IsActionPressed("move_sprint"))
                {
                    return State.Sprinting;
                }
                return State.Walking;
            }

            return State.Idle;
        }

        /**
            Method meant to be overridden by a player subclass.
        */
        public virtual Vector3 CalculateMovement(double delta) {return Vector3.Zero;}
    }
}