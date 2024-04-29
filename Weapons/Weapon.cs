using System;
using System.Runtime.CompilerServices;
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
        public float recoil = 0.05f;
        [Export]
        public float recoilSpeed = 0.1f;
        [Export]
        public float recoilWeight = 8f;
        [Export]
        public float recoilTorque = 15f;
        [Export]
        public float recoilKickback = 0.05f;

        [Export]
        public string name = "";
        [Export]
        public int magazineSize = 0;

        private int currentAmmoInMagazine = 0;

        private Tween tween;

        public override void _Ready()
        {
            currentAmmoInMagazine = magazineSize;
        }

        public void shoot()
        {
            if (currentAmmoInMagazine > 0 || magazineSize == -1)
            {
                currentAmmoInMagazine -= 1;
                shootAnimation();
            }

            GD.Print("current ammo: ", currentAmmoInMagazine);
        }

        public void shootAnimation()
        {
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }

            tween = CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Sine).SetParallel();
            tween.TweenProperty(this, "position:y", Position.Y + recoil, recoilSpeed / recoilWeight);
            tween.TweenProperty(this, "rotation_degrees:x", RotationDegrees.X + recoilTorque, recoilSpeed / recoilWeight);
            tween.TweenProperty(this, "position:z", Position.Z + recoilKickback, recoilSpeed / recoilWeight);
            tween.Chain().TweenProperty(this, "position:y", 0, recoilSpeed);
            tween.Chain().TweenProperty(this, "rotation_degrees:x", 0, recoilSpeed);
            tween.Chain().TweenProperty(this, "position:z", 0, recoilSpeed);
        }

        public int getCurrentAmmoInMagazine()
        {
            return currentAmmoInMagazine;
        }
    }
}