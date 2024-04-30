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
			GD.Print("damager: ", damager);
			damager.damageBehaviour(this);
		}
	}
}
