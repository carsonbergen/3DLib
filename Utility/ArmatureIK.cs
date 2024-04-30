using Godot;
using System;

namespace ThreeDLib
{
	public partial class ArmatureIK : Node3D
    {
        [Export]
        public SkeletonIK3D leftArmIK;
        [Export]
        public SkeletonIK3D rightArmIK;
        [Export]
        public Marker3D leftArmTarget;
        [Export]
        public Marker3D rightArmTarget;

        public void Setup()
        {
            if (leftArmIK == null || rightArmIK == null || leftArmTarget == null || rightArmTarget == null) {
                GD.PushError(
                    "One or more necessary nodes are null. Necessary nodes:\n", 
                    "leftArmIK:\t", leftArmIK,
                    "rightArmIK:\t", rightArmIK,
                    "leftArmTarget:\t", leftArmTarget,
                    "rightArmTarget:\t", rightArmTarget
                );
            } else {
                // Set up
                leftArmIK.TargetNode = leftArmTarget.GetPath();
                rightArmIK.TargetNode = rightArmTarget.GetPath();
                leftArmIK.Start();
                rightArmIK.Start();

            }
        }
    }
}