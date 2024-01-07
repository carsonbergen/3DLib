using System.Diagnostics;
using Godot;

namespace ThreeDLib
{
	public partial class HoverBike : RigidBody3D
	{
		[Signal]
		public delegate void PlayerInRangeEventHandler();
		[Signal]
		public delegate void PlayerOutOfRangeEventHandler();

		[ExportCategory("")]
		[Export]
		public bool isControlled = false;
		[Export]
		public bool isInteractable = true;
		[ExportCategory("")]
		[Export]
		public Node3D model;
		[Export]
		public Area3D interactionArea;
		[Export]
		public Marker3D seat;

		[ExportCategory("Vehicle & engine properties")]
		[ExportGroup("Rotation speeds")]
		[Export]
		public float leanSpeed = 10f;
		[Export]
		public float baseTurnSpeed = 5f;
		[Export]
		public float boostTurnSpeed = 2.5f;
		[Export]
		public float resetSpeed = 10f;
		[ExportGroup("Movement speeds")]
		[Export]
		public float baseSpeed = 50f;
		[Export]
		public float boostSpeed = 100f;
		[Export]
		public float steeringSpeed = 1f;
		[Export]
		public float brakingSpeed = 200f;
		[Export]
		public float maxBaseSpeed = 10f;
		[Export]
		public float maxBoostSpeed = 20f;
		[ExportCategory("Physics")]
		[Export]
		public RayCast3D groundCast;

		private float speed = 0f;
		private float turnSpeed = 0f;

        public override void _PhysicsProcess(double delta)
		{
			var engineThrottleInput = Input.GetAxis("vehicle_backward", "vehicle_forward");
			var steeringInput = Input.GetAxis("vehicle_right", "vehicle_left");

			if (Input.IsActionPressed("vehicle_boost"))
			{
				speed = boostSpeed;
				turnSpeed = boostTurnSpeed;
			}
			else
			{
				speed = baseSpeed;
				turnSpeed = baseTurnSpeed;
			}

			if (steeringInput != 0f)
				RotateY(steeringInput * (float)delta * turnSpeed);

			if (groundCast.IsColliding())
				ApplyCentralForce(GlobalTransform.Basis.Z * engineThrottleInput * speed);
			else
				ApplyCentralForce(GlobalTransform.Basis.Z * speed);
		}

		public override void _Process(double delta)
		{
			var steeringInput = Input.GetAxis("vehicle_right", "vehicle_left");

			// Reset model roll
			if (steeringInput == 0f || !groundCast.IsColliding())
				model.RotationDegrees = model.RotationDegrees with
				{
					X = Mathf.Lerp(model.RotationDegrees.X, 0f, (float)delta * resetSpeed)
				};
			// Rotate model
			else
			{
				model.RotationDegrees = model.RotationDegrees with
				{
					X = Mathf.Lerp(
									model.RotationDegrees.X,
									model.RotationDegrees.X + (45 * steeringInput),
									(float)delta * leanSpeed
								)
				};
				model.RotationDegrees = model.RotationDegrees with
				{
					X = Mathf.Clamp(model.RotationDegrees.X, -15, 15)
				};
			}
		}

		// public void OnAreaEntered(Area3D area)
		// {
		// 	if (area.IsInGroup("Player"))
		// 	{
		// 		GetNode<GameEventHandler>("/root/GameEventHandler").PlayerInRangeOfInteractableObject(this);
		// 	}
		// }

		// public void OnAreaExited(Area3D area)
		// {
		// 	if (area.IsInGroup("Player"))
		// 	{
		// 		GetNode<GameEventHandler>("/root/GameEventHandler").PlayerOutOfRangeOfInteractableObject(this);
		// 	}
		// }
	}
}
