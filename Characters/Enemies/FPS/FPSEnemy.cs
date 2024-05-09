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

		public void applyDamagers(Array<Damager> damagers, Area3D area, int passThroughAmount)
		{
			foreach (FPSDamager damager in damagers)
			{
				// GD.Print(this, "'s health before:\t", health);
				// GD.Print("\nenemy: ", this);
				// GD.Print("area: '", area, "'\t", area == headArea, "==\t headArea: '", headArea, "'");
				// GD.Print("area: '", area, "'\t", area == bodyArea, "==\t bodyArea: '", bodyArea, "'");
				damager.damageBehaviour(this, area.Equals(headArea), passThroughAmount);
				// GD.Print(this, "'s health after:\t", health, "\n");
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