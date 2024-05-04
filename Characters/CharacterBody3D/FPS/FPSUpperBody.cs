using Godot;
using ThreeDLib;

namespace FPS
{
    public partial class FPSUpperBody : Node3D
    {
        [Export]
        public FPSCharacterBody3D player;
        [Export]
        public Node3D model;
        [Export]
        public Camera3D camera;

        [Export]
        public FPSWeaponHolder weaponHolder;

        [ExportGroup("Procedural Animation Settings")]
        [Export]
        public float walkingAnimationSpeed = 0.35f;
        [Export]
        public float sprintingAnimationSpeed = 0.25f;

        [ExportSubgroup("Gun Holder Position Settings")]
        [Export]
        public float defaultY = -0.375f;
        [Export]
        public float upY = -0.325f;
        [Export]
        public float downY = -0.4f;
        private Tween tween;

        public override void _Ready()
        {
            base._Ready();
            if (weaponHolder == null || player == null)
            {
                GD.PrintErr("No player provided to UpperBody node.");
                GetTree().Quit();
            }
        }

        public override void _Process(double delta)
        {
            if (player.currentState == State.Walking && tween == null)
            {
                MovementAnimation(walkingAnimationSpeed);
            }
            else if (player.currentState == State.Sprinting && tween == null)
            {
                MovementAnimation(sprintingAnimationSpeed);
            }
            if (tween != null && !tween.IsRunning())
            {
                tween = null;
            }
        }

        public void MovementAnimation(float speed)
        {
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }
            tween = this.CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Sine).SetParallel();
            tween.TweenProperty(weaponHolder, "position:y", upY, speed);
            tween.Chain().TweenProperty(weaponHolder, "position:y", downY, speed).SetTrans(Tween.TransitionType.Bounce);
            tween.Chain().TweenProperty(weaponHolder, "position:y", defaultY, speed);
        }
    }
}