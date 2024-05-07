using System;
using System.Runtime.CompilerServices;
using FPS;
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
        public int totalAmmo = 0;
        [Export]
        public float range = 1000f;
        [Export]
        public float recoilResetRate = 0.1f;
        [Export]
        public bool hasScope = false;
        [Export]
        public float scopeReticleLength = 100f;
        [Export]
        public int zoom = 1;
        [Export]
        public float adsSpeed = 1f;
        [Export]
        public float baseSpread = 10f;
        [Export]
        public float adsAccuracy = 2.5f;
        [Export]
        public float hipFireAccuracy = 5f;
        [Export]
        public float recoilFactor = 1f;
        [Export]
        public float reloadSpeed = 1f;
        [Export]
        public int enemyPassThrough = 3;

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
        [Export]
        public CanvasLayer scopeLayer;

        private int currentAmmoInMagazine = 0;

        private int totalAmmoLeft = 0;

        private Tween tween;

        private Tween reloadTween;

        private float currentTime = 0;

        private float bulletRadius = 0f;

        private float accuracy = 0f;

        private bool scopedIn = false;

        public override void _Ready()
        {
            if (scopeLayer != null)
                scopeLayer.Visible = false;

            accuracy = baseSpread + hipFireAccuracy;

            if (pivot == null)
                pivot = GetChild<Node3D>(0);

            pivot.Position = pivot.Position with
            {
                Z = defaultZPosition
            };
            currentAmmoInMagazine = magazineSize;
            totalAmmoLeft = totalAmmo;
        }

        public override void _Process(double delta)
        {
            if (reloadTween == null)
            {
                Position = Position with
                {
                    Z = Mathf.Clamp(Position.Z, 0, maxZ)
                };

                RotationDegrees = RotationDegrees with
                {
                    X = Mathf.Clamp(RotationDegrees.X, 0, maxXRotation)
                };
            }

            currentTime += (float)delta;

            if (Visible)
            {
                if (reloadTween != null)
                {
                    if (!reloadTween.IsValid()) reloadTween = null;
                    else if (reloadTween.IsValid())
                    {
                        if (!reloadTween.IsRunning()) reloadTween.Play();
                    }
                }
                else
                {
                    if (Input.IsActionPressed("ads"))
                    {
                        scopedIn = true;
                        ads(scopedIn);
                    }
                    else if (Input.IsActionJustReleased("ads"))
                    {
                        scopedIn = false;
                        ads(scopedIn);
                    }

                    bulletRadius = (scopedIn ? 0 : baseSpread) + accuracy + (Position.Z * 100 * (recoil + accuracy) * Position.Z * recoilFactor);
                    crosshair.adjustSpread(bulletRadius);

                    // Reset back to default
                    if (currentTime > recoilResetRate)
                    {
                        bulletRaycast.TargetPosition = new Vector3(0, 0, -range);
                    }

                    if (scopedIn)
                    {
                        accuracy = Mathf.MoveToward(
                            accuracy,
                            adsAccuracy,
                            (float)delta * adsSpeed * 100
                        );
                        if (fullyScopedIn())
                        {
                            if (scopeLayer != null)
                                scopeLayer.Visible = true;
                            if (hasScope)
                            {
                                crosshair.tempAdjustLength(scopeReticleLength);
                                crosshair.tempTShape();
                            }
                        }
                    }
                    else
                    {
                        if (scopeLayer != null)
                            scopeLayer.Visible = false;
                        accuracy = baseSpread + hipFireAccuracy;
                    }
                }
            }
            else
            {
                if (reloadTween != null && reloadTween.IsRunning()) reloadTween.Pause();
                scopedIn = false;
                if (scopeLayer != null) scopeLayer.Visible = false;
                pivot.Visible = true;
                accuracy = hipFireAccuracy;
            }
        }

        public void ads(bool input)
        {
            if (hasScope && fullyScopedIn())
            {
                // Handle logic for when there is a scope
                if (scopedIn)
                {
                    pivot.Visible = false;
                }
                else
                {
                    pivot.Visible = true;
                }
            }
            else
            {
            }
        }

        public bool fullyScopedIn()
        {
            return accuracy == adsAccuracy;
        }

        public void setScopedIn(bool value)
        {
            scopedIn = value;
        }

        public bool isScopedIn()
        {
            return scopedIn;
        }

        public void shoot()
        {
            if (currentTime > fireRate && reloadTween == null)
            {
                if (currentAmmoInMagazine > 0 || magazineSize == -1)
                {
                    var collisionPoint = bulletRaycast.GetCollisionPoint();
                    var spaceState = GetWorld3D().DirectSpaceState;
                    var fromPoint = bulletRaycast.GlobalPosition;
                    var toPoint = collisionPoint + (
                        range / fromPoint.DistanceTo(collisionPoint)
                    ) * (collisionPoint - fromPoint);

                    var vertices = new Vector3[]
{
                        fromPoint,
                        toPoint
};

                    // Initialize the ArrayMesh.
                    var arrMesh = new ArrayMesh();
                    var arrays = new Godot.Collections.Array();
                    arrays.Resize((int)Mesh.ArrayType.Max);
                    arrays[(int)Mesh.ArrayType.Vertex] = vertices;

                    // Create the Mesh.
                    arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Lines, arrays);
                    var mesh = new MeshInstance3D
                    {
                        Mesh = arrMesh
                    };
                    GetTree().Root.AddChild(mesh);

                    for (int i = 0; i < enemyPassThrough; i++)
                    {
                        var query = PhysicsRayQueryParameters3D.Create(fromPoint, toPoint);
                        query.CollideWithAreas = true;
                        query.CollideWithBodies = false;
                        Godot.Collections.Dictionary result = spaceState.IntersectRay(query);
                        result.TryGetValue("collider", out Variant collider);
                        if (((Node3D)collider) != null)
                        {
                            if (((Node3D)collider).GlobalPosition == fromPoint)
                                break;
                            fromPoint = ((Node3D)collider).GlobalPosition;

                            if (((Node3D)collider) is Area3D area)
                            {
                                var parent = area.GetParent();
                                if (parent is FPSEnemy)
                                {
                                    ((FPSEnemy)parent).applyDamagers(damagers, area);
                                }
                            }
                        }
                        else break;
                    }

                    shootAnimation();

                    GD.Randomize();
                    bulletRaycast.TargetPosition = bulletRaycast.TargetPosition with
                    {
                        X = (float)GD.RandRange(-bulletRadius, bulletRadius),
                        Y = (float)GD.RandRange(-bulletRadius, bulletRadius)
                    };
                    currentAmmoInMagazine -= 1;
                    currentTime = 0;
                }
            }
        }

        public void reload()
        {
            if (reloadTween == null && currentAmmoInMagazine < magazineSize && totalAmmoLeft > 0)
            {
                reloadAnimation();
                int ammoToAdd = 0;

                if (magazineSize > totalAmmoLeft)
                {
                    ammoToAdd = totalAmmoLeft;
                }
                else
                {
                    ammoToAdd = magazineSize - currentAmmoInMagazine;
                }
                currentAmmoInMagazine += ammoToAdd;
                totalAmmoLeft -= ammoToAdd;
                reloadTween.Finished += () => reloadTween = null;
            }
        }

        public void reloadAnimation()
        {
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }

            if (reloadTween != null)
            {
                reloadTween.Kill();
                reloadTween = null;
            }

            reloadTween = CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Sine).SetParallel();
            reloadTween.TweenProperty(this, "position", Position + new Vector3(0f, -0.15f, 0.1f), reloadSpeed / 2);
            reloadTween.TweenProperty(this, "rotation_degrees", RotationDegrees + new Vector3(35, 24, 25), reloadSpeed / 2);
            reloadTween.Chain().TweenProperty(this, "rotation_degrees", Vector3.Zero, reloadSpeed);
            reloadTween.Chain().TweenProperty(this, "position", Vector3.Zero, reloadSpeed);
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

        public int getTotalAmmoLeft()
        {
            return totalAmmoLeft;
        }
    }
}