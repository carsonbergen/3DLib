using Godot;
using System;
using ThreeDLib;

public partial class Enemy : RigidBody3D
{
	[Export]
	public float health = 100f;

	public void applyDamagers(Godot.Collections.Array<Damager> damagers)
	{
		foreach (Damager damager in damagers)
		{
			// GD.Print("damager: ", damager);
			GD.Print(this, "'s health before:\t", health);

			damager.damageBehaviour(this);
			GD.Print(this, "'s health after:\t", health, "\n");
		}

		if (health <= 0)
		{
			Visible = false;
			SetProcess(false);
			SetPhysicsProcess(false);
			DisableMode = DisableModeEnum.Remove;
			QueueFree();
		}
	}
}
