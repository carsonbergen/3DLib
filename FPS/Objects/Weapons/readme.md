# FPS
## FPSDamager
Contains the logical for damaging enemies, including:
- passthrough damage reduction amount,
- headshot bonus.

## Scope
Draws a scope reticle on the screen.

## Weapon
Contains all logic for weapons, including:
- weapon attributes,
- armature targets,
- modifying the crosshair,


```
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

                    Node3D lastCollider = null;

                    for (int i = 0; i < enemyPassThrough; i++)
                    {
                        var query = PhysicsRayQueryParameters3D.Create(fromPoint, toPoint);
                        query.CollideWithAreas = true;
                        query.CollideWithBodies = false;
                        Godot.Collections.Dictionary result = spaceState.IntersectRay(query);
                        result.TryGetValue("collider", out Variant collider);

                        if (((Node3D)collider) != null)
                        {
                            if (((Node3D)collider) is Area3D area)
                            {
                                var parent = area.GetParent();
                                if (parent is FPSEnemy enemy)
                                {
                                    if (lastCollider == ((Node3D)collider)) break;

                                    lastCollider = (Node3D)collider;
                                    enemy.applyDamagers(damagers, area, i);
                                }
                            }
                            fromPoint = ((Node3D)collider).GlobalPosition;
                        }
                        else break;
                    }
```