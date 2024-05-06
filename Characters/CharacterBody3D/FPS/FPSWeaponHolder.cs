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
        public FPSUpperBody upperBody;

        [Export]
        public int maxWeapons = 2;

        [Export]
        public Vector3 defaultPosition;

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

        public Weapon currentWeapon;

        private Vector2 mouseMovement = new Vector2(0, 0);

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
                if (Input.IsActionJustPressed("next_weapon"))
                {
                    SwitchWeapon(true);
                }
                else if (Input.IsActionJustPressed("previous_weapon"))
                {
                    SwitchWeapon(false);
                }
                else if (Input.IsActionJustPressed("reload_weapon"))
                {
                    currentWeapon.reload();
                }
            }

            if (@event is InputEventMouseButton)
            {
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (currentWeapon.automatic)
            {
                if (Input.IsActionPressed("shoot_weapon"))
                {
                    currentWeapon.shoot();
                }
            }
            else
            {
                if (Input.IsActionJustPressed("shoot_weapon"))
                {
                    currentWeapon.shoot();
                }
            }

            if (currentWeapon.isScopedIn())
            {
                if (currentWeapon.hasScope && currentWeapon.fullyScopedIn())
                {
                    upperBody.model.Visible = false;
                    upperBody.camera.Fov = currentWeapon.zoom;
                }
                Position = Position with
                {
                    X = (float)Mathf.Lerp(Position.X, 0, delta * currentWeapon.adsSpeed)
                };
            }
            else
            {
                upperBody.model.Visible = true;
                upperBody.camera.Fov = upperBody.player.fov;
                Position = Position with
                {
                    X = (float)Mathf.Lerp(Position.X, defaultPosition.X, delta * currentWeapon.adsSpeed)
                };
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

        public void SwitchWeapon(bool direction)
        {
            // Reset current weapon and upperbody
            upperBody.model.Visible = true;
            upperBody.camera.Fov = upperBody.player.fov;
            Position = defaultPosition;
            currentWeapon.setScopedIn(false);
            currentWeapon.Visible = false;
            var weaponIndex = currentWeapon.GetIndex();
            if (direction)
            {
                weaponIndex = currentWeapon.GetIndex() + 1;
                if (weaponIndex > (maxWeapons - 1) || weaponIndex > (GetChildCount() - 1))
                {
                    weaponIndex = 0;
                }
            }
            else
            {
                weaponIndex = currentWeapon.GetIndex() - 1;
                if ((weaponIndex < 0) || weaponIndex > (GetChildCount() - 1))
                {
                    weaponIndex = GetChildCount() - 1;
                }
            }
            currentWeapon = GetChild<Weapon>(weaponIndex);
            UpdateArmatureIK();
            currentWeapon.setScopedIn(false);
            currentWeapon.Visible = true;
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
                float weight = currentWeapon.weight;
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