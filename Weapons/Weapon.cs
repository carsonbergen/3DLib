using System;
using System.Runtime.CompilerServices;
using Godot;

namespace ThreeDLib
{
    public partial class Weapon : Node3D
    {
        [ExportCategory("External Nodes")]
        [Export]
        public Crosshair crosshair;
        [Export]
        public RayCast3D bulletRaycast;

        [ExportCategory("IK Targets")]
        [Export]
        public Marker3D leftArmTarget;
        [Export]
        public Marker3D rightArmTarget;

        [ExportCategory("Animation Settings")]
        [Export]
        public float defaultZPosition;
        [Export]
        public float weight = 5f;

        [ExportCategory("Gun Data")]
        [Export]
        public float fireRate = 0.05f;
        [Export]
        public string name = "";
        [Export]
        public bool automatic = false;
        [Export]
        public int magazineSize = 0;
        [Export]
        public float range = 1000f;
        [Export]
        public float recoilResetRate = 0.1f;
        [Export]
        public bool hasScope = false;
        [Export]
        public float adsSpeed = 1f;
        [Export]
        public float adsAccuracy = 2.5f;
        [Export]
        public float hipFireAccuracy = 5;

        [Export]
        public Godot.Collections.Array<Damager> damagers { get; set; }

        [ExportCategory("Recoil Animation Values")]
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
        public float maxZ = 0.25f;
        [Export]
        public float maxXRotation = 65f;

        [ExportCategory("Other Internal Nodes")]
        [Export]
        public Node3D pivot;

        private int currentAmmoInMagazine = 0;

        private Tween tween;

        private float currentTime = 0;

        private float bulletRadius = 0f;

        private float accuracy = 0f;

        private bool scopedIn = false;

        public override void _Ready()
        {
            accuracy = hipFireAccuracy;
            if (pivot == null)
            {
                pivot = GetChild<Node3D>(0);
            }
            pivot.Position = pivot.Position with
            {
                Z = defaultZPosition
            };
            currentAmmoInMagazine = magazineSize;
        }

        public override void _Process(double delta)
        {
            Position = Position with
            {
                Z = Mathf.Clamp(Position.Z, 0, maxZ)
            };

            RotationDegrees = RotationDegrees with
            {
                X = Mathf.Clamp(RotationDegrees.X, 0, maxXRotation)
            };

            currentTime += (float)delta;

            if (Visible)
            {
                if (Input.IsActionPressed("ads"))
                {
                    scopedIn = true;
                }
                else if (Input.IsActionJustReleased("ads"))
                {
                    scopedIn = false;
                }

                bulletRadius = accuracy + (Position.Z * 100 * (recoil + accuracy) * Position.Z);
                crosshair.adjustSpread(bulletRadius);

                if (currentTime > recoilResetRate)
                {
                    bulletRaycast.TargetPosition = new Vector3(0, 0, -range);
                }

                if (scopedIn)
                {
                    accuracy = adsAccuracy;
                }
                else
                {
                    accuracy = hipFireAccuracy;
                }
            }
            else
            {
                scopedIn = false;
            }
        }

        public void ads(bool input)
        {
            scopedIn = input;
            if (hasScope)
            {
                // Handle logic for when there is a scope
            }
            else
            {
            }
        }

        public bool isScopedIn()
        {
            return scopedIn;
        }

        public void shoot()
        {
            if (currentTime > fireRate)
            {
                if (currentAmmoInMagazine > 0 || magazineSize == -1)
                {
                    shootAnimation();

                    GD.Randomize();
                    bulletRaycast.TargetPosition = bulletRaycast.TargetPosition with
                    {
                        X = (float)GD.RandRange(-bulletRadius, bulletRadius),
                        Y = (float)GD.RandRange(-bulletRadius, bulletRadius)
                    };

                    currentAmmoInMagazine -= 1;

                    currentTime = 0;

                    var obj = bulletRaycast.GetCollider();
                    // GD.Print(obj);
                    if (obj is Enemy enemy)
                    {
                        enemy.applyDamagers(damagers);
                    }
                }
            }
        }

        public void reload()
        {
            currentAmmoInMagazine = magazineSize;
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