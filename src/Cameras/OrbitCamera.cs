using Godot;
using System;
using System.Resources;
using ThreeDLib;

public partial class OrbitCamera : Node3D
{
	[ExportCategory("External nodes")]
	[Export]
	public Node3D target;
	[ExportCategory("Internal nodes")]
	[Export]
	public Camera3D camera;
	[Export]
	public SpringArm3D pivot;
	[ExportCategory("Camera properties")]
	[Export]
	public float movementSpeed = 15f;
	[Export]
	public float cameraSensitivity = 1000f / 1000f;
	[Export]
	public float cameraAccelerationSpeed = 0.5f;
	[Export]
	public float cameraAccelerationAmount = 0.5f;
	[Export]
	public float baseCameraUpdateSpeed = 0.5f;
	[Export]
	public float fastCameraUpdateSpeed = 0.75f;
	[Export]
	public bool inverted = false;
	[ExportCategory("Pivot arm properties")]
	[Export]
	public float minDistance = 0.5f;
	[Export]
	public float minXRotation = -65f;
	[Export]
	public float maxXRotation = 45f;

	private double time = 0;

	private float internalCameraSensitivity = 0;
	private float maxInternalCameraSensitivity = 0;

	/**
		TODO:
			Add mouse movement to rotate camera
	*/

	public override void _Ready()
	{
		// Set initial values
		internalCameraSensitivity = cameraSensitivity;
		maxInternalCameraSensitivity = cameraSensitivity + cameraAccelerationAmount;

		if (camera == null)
		{
			handleError("Orbit camera requires camera node");
		}

		if (target == null)
		{
			handleError("Target node cannot be null");
		}

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = GlobalPosition.Lerp(target.GlobalPosition, (float) delta * movementSpeed);

		var look = Input.GetAxis("camera_rotate_left", "camera_rotate_right");
		if (time > cameraAccelerationSpeed)
			internalCameraSensitivity = cameraSensitivity + (cameraAccelerationAmount / 100f);
		
		if (look > 0)
		{
			time += delta;
			if (inverted)
				RotateY(Mathf.DegToRad(internalCameraSensitivity));
			else
				RotateY(Mathf.DegToRad(-internalCameraSensitivity));
		}
		else if (look < 0)
		{
			time += delta;
			if (inverted)
				RotateY(Mathf.DegToRad(-internalCameraSensitivity));
			else
				RotateY(Mathf.DegToRad(internalCameraSensitivity));
		}
		else
		{
			internalCameraSensitivity = cameraSensitivity;
			time = 0;
		}
		Mathf.Clamp(internalCameraSensitivity, cameraSensitivity, maxInternalCameraSensitivity);
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion mouseEvent = (InputEventMouseMotion) @event;
			if (Input.MouseMode == Input.MouseModeEnum.Captured)
			{
				RotateY((float) -mouseEvent.Relative.X * cameraSensitivity * 0.001f);
				pivot.RotateX((float) -mouseEvent.Relative.Y * cameraSensitivity * 0.001f);
				pivot.RotationDegrees = pivot.RotationDegrees with {
					X = Mathf.Clamp(pivot.RotationDegrees.X, minXRotation, maxXRotation)
				};
			}
		}
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
		{
			InputEventKey e = (InputEventKey) @event;
			if (e.IsActionPressed("open_command_terminal"))
			{
				if (Input.MouseMode != Input.MouseModeEnum.Captured)
					Input.MouseMode = Input.MouseModeEnum.Captured;
				else
                    Input.MouseMode = Input.MouseModeEnum.Visible;
            }
		}
    }

    private void handleError(string message)
	{
		GD.PrintErr(message);
		GetTree().Quit();
	}
}
