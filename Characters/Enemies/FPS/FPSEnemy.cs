using Godot;
using Godot.Collections;
using ThreeDLib;

namespace FPS
{
	public partial class FPSEnemy : Enemy
	{
		[Export]
		public float speed = 5f;

		[Export]
		public Area3D headArea;
		[Export]
		public Area3D bodyArea;
		[Export]
		public NavigationAgent3D navAgent;

		public override async void _PhysicsProcess(double delta) {
			var curLocation = GlobalTransform.Origin;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			var nextLocation = navAgent.GetNextPathPosition();
			var newVelocity = (nextLocation - curLocation).Normalized() * speed;
			
			navAgent.Velocity = newVelocity;
		}

		public void _OnNavigationAgent3DVelocityComputed(Vector3 safeVelocity) {
			Velocity = Velocity.MoveToward(safeVelocity, 0.25f);
			MoveAndSlide();
		}

		public void applyDamagers(Array<Damager> damagers, Area3D area, int passThroughAmount)
		{
			foreach (FPSDamager damager in damagers)
			{
				damager.damageBehaviour(this, area.Equals(headArea), passThroughAmount);
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

		public void updateTargetLocation(Vector3 targetPosition) {
			navAgent.TargetPosition = targetPosition;
		}
	}
}