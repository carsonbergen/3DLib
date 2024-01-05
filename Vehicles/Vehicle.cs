using Godot;

namespace ThreeDLib
{
    public partial class Vehicle : VehicleBody3D
    {
        [Signal]
        public delegate void PlayerInRangeEventHandler();
        [Signal]
        public delegate void PlayerOutOfRangeEventHandler();

        [Export]
        public Node3D model;
    }
}