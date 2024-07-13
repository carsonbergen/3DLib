using FPS;
using Godot;
using System;
using ThreeDLib;

public partial class EnemyManager : Node3D
{
	[Export]
	public PlayerCharacterBody3D player;

    public override void _PhysicsProcess(double delta)
	{
		GetTree().CallGroup("Enemies", "updateTargetLocation", player.GlobalTransform.Origin);
	}
}
