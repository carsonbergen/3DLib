using Godot;
using Godot.Collections;
using ThreeDLib;

namespace FPS
{
	public partial class FPSEnemy : Enemy
	{
		[Export]
		public Area3D headArea;
		[Export]
		public Area3D bodyArea;

		public void applyDamagers(Array<Damager> damagers, Area3D area)
		{
			foreach (FPSDamager damager in damagers)
			{
				// GD.Print("damager: ", damager);
				GD.Print(this, "'s health before:\t", health);
				damager.damageBehaviour(this, area == headArea);
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
}