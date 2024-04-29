using Godot;
using System;
using ThreeDLib;

namespace FPS
{
    public partial class FPSWeaponHolder : Node3D
    {
        [Export]
        public ArmatureIK armatureIK;

        [Export]
        public int maxWeapons = 2;

        [ExportGroup("Weapon Sway Settings")]
        [Export]
        public double swayAmount = 25f;
        [Export]
        public double swayResetSpeed = 10f;
        [Export]
        public Vector2 swayThresholds = new Vector2(1.0f, 1.0f);
        [Export]
        public Vector2 swayVector = new Vector2(0.4f, 0.4f);
        [Export]
        public Vector2 swayClamp = new Vector2(15f, 15f);


        private Vector2 mouseMovement = new Vector2();

        private Weapon currentWeapon;

        public override void _Ready()
        {
            // Get first weapon
            Weapon weapon = GetChild<Weapon>(0);
            weapon.Visible = true;
            currentWeapon = weapon;
            // Setup armature IK
            UpdateArmatureIK();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion)
            {
                mouseMovement = new Vector2(
                    ((InputEventMouseMotion)@event).Relative.Y,
                    ((InputEventMouseMotion)@event).Relative.X
                );
            }

            if (@event is InputEventKey)
            {
                if (Input.IsActionJustPressed("switch_weapon"))
                {
                    currentWeapon.Visible = false;
                    var weaponIndex = currentWeapon.GetIndex() + 1;

                    if (weaponIndex > (maxWeapons - 1) || weaponIndex > (GetChildCount() - 1))
                    {
                        weaponIndex = 0;
                    }

                    currentWeapon = GetChild<Weapon>(weaponIndex);
                    currentWeapon.Visible = true;
                    UpdateArmatureIK();

                    GD.Print(currentWeapon.getCurrentAmmoInMagazine());
                }
            }

            if (@event is InputEventMouseButton)
            {
                if (Input.IsActionJustPressed("shoot_weapon"))
                {
                    currentWeapon.shoot();
                }
            }
        }

        public override void _Process(double delta)
        {
            if (mouseMovement.X > swayThresholds.X || mouseMovement.X < -swayThresholds.X)
                Sway(delta);
            else if (mouseMovement.Y > swayThresholds.Y || mouseMovement.Y < -swayThresholds.Y)
                Sway(delta);
            else
                RotationDegrees = RotationDegrees.Lerp(Vector3.Zero, (float)(swayResetSpeed * delta));
        }

        public void UpdateArmatureIK()
        {
            armatureIK.leftArmTarget = currentWeapon.leftArmTarget;
            armatureIK.rightArmTarget = currentWeapon.rightArmTarget;
            armatureIK.Setup();
        }

        public void Sway(double delta)
        {
            if (GetChildCount() > 0)
            {
                // float weight = (float)GetChild<Node3D>(currentWeapon).GetMeta("weight");
                float weight = 5f;
                RotationDegrees = RotationDegrees with
                {
                    X = Mathf.Lerp(
                        RotationDegrees.X,
                        Mathf.Clamp(
                            RotationDegrees.X + (swayVector.X * mouseMovement.X * weight),
                            -swayClamp.X,
                            swayClamp.Y
                        ),
                        (float)(swayAmount * delta)
                    ),
                    Y = Mathf.Lerp(
                        RotationDegrees.Y,
                        Mathf.Clamp(
                            RotationDegrees.Y + (swayVector.Y * mouseMovement.Y * weight),
                            -swayClamp.Y,
                            swayClamp.Y
                        ),
                        (float)(swayAmount * delta)
                    )
                };
            }
        }
    }
}