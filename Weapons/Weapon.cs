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
        public float recoil = 0f;
        [Export]
        public float recoilSpeed = 0f;
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
            if (currentAmmoInMagazine > 0)
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
            var oldY = Position.Y;

            tween = CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Sine).SetParallel();
            tween.TweenProperty(this, "position:y", Position.Y + recoil, recoilSpeed);
            tween.Chain().TweenProperty(this, "position:y", oldY, recoilSpeed * 2);
        }

        public int getCurrentAmmoInMagazine()
        {
            return currentAmmoInMagazine;
        }
    }
}