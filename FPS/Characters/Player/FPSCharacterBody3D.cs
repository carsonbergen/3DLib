using FPS;
using Godot;
using System;
using System.Threading.Tasks;

namespace ThreeDLib
{
	public partial class FPSCharacterBody3D : PlayerCharacterBody3D
	{
		[Export]
		public PlayerSettings playerSettings;

		[Export]
		public PlayerAttributes playerAttributes;

		// Holds the camera
		[Export]
		public FPSUpperBody upperBody = null;

		private float movementFactor = 1f;
		private Vector3 jumpDirection;

		private float mouseSensitivity = 0f;

		public override void _Ready()
		{
			Task<PlayerSettings> settings = loadSavedSettings();

			mouseSensitivity = playerSettings.lookSensitivity;
			Setup();
			Input.MouseMode = Input.MouseModeEnum.Captured;
			GD.Print("Player is ready");
		}

		public override void _PhysicsProcess(double delta)
		{
			currentState = GetState();
			if (currentState != State.InAir)
			{
				if (upperBody.isCrouching())
				{
					currentState = State.Crouching;
				}

				if (upperBody.weaponHolder.currentWeapon.isScopedIn())
				{
					currentState = State.ADSing;
				}
			}

			if (currentState == State.ADSing)
			{
				mouseSensitivity = playerSettings.adsSensitivity;
			}
			else
			{
				mouseSensitivity = playerSettings.lookSensitivity;
			}

			if (isControlled && currentState != State.InVehicle)
				Velocity = CalculateMovement(delta);
			MoveAndSlide();
		}

		public override void _Input(InputEvent @event)
		{
			// Calculate mouse capture
			if ((@event is InputEventMouseMotion eventMouseMotion) && (Input.MouseMode == Input.MouseModeEnum.Captured))
			{
				RotateY(Mathf.DegToRad(-eventMouseMotion.Relative.X * mouseSensitivity));
				upperBody.RotateX(Mathf.DegToRad(-eventMouseMotion.Relative.Y * mouseSensitivity));
				upperBody.RotationDegrees = upperBody.RotationDegrees with { X = Mathf.Clamp(upperBody.RotationDegrees.X, -90, 89) };
			}
			// Disable mouse capture
			if (@event is InputEventKey inputEventKey)
			{
				if (inputEventKey.IsActionPressed("open_menu"))
				{
					if (Input.MouseMode == Input.MouseModeEnum.Captured)
						Input.MouseMode = Input.MouseModeEnum.Visible;
					else
						Input.MouseMode = Input.MouseModeEnum.Captured;
				}
			}
		}

		public static Task<PlayerSettings> loadSavedSettings()
		{
			var newConfig = new ConfigFile();
			var err = newConfig.Load("player_settings.cfg");
			

			return null;
		}

		public override Vector3 CalculateMovement(double delta)
		{
			Vector3 velocity = Velocity;

			if (currentState == State.InAir)
			{
				velocity.Y -= gravity * (float)delta;
				movementFactor = 0.25f;
			}
			else if (currentState != State.InAir)
			{
				movementFactor = 1f;
			}

			if (currentState == State.Walking)
				speed = Mathf.Lerp(speed, playerAttributes.walkSpeed, (float)delta * speedChangeFactor);
			else if (currentState == State.Sprinting)
				speed = Mathf.Lerp(speed, playerAttributes.sprintSpeed, (float)delta * speedChangeFactor);
			else if (currentState == State.ADSing)
				speed = Mathf.Lerp(speed, playerAttributes.adsWalkSpeed, (float)delta * speedChangeFactor);
			else if (currentState == State.Crouching)
				speed = Mathf.Lerp(speed, playerAttributes.crouchSpeed, (float)delta * speedChangeFactor);

			Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward") * movementFactor;

			Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

			if (Input.IsActionJustPressed("move_jump") && currentState != State.InAir)
			{
				velocity.Y = jumpVelocity;
				jumpDirection = direction;
			}
			else if (currentState != State.InAir)
			{
				jumpDirection = Vector3.Zero;
			}

			if (jumpDirection != Vector3.Zero)
			{
				velocity.X = (jumpDirection.X * speed * playerAttributes.jumpDistance / 2) + (direction.X * speed * playerAttributes.inAirMovementFactor);
				velocity.Z = (jumpDirection.Z * speed * playerAttributes.jumpDistance / 2) + (direction.Z * speed * playerAttributes.inAirMovementFactor);
			}
			else if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * speed;
				velocity.Z = direction.Z * speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
			}
			return velocity;
		}
	}
}
