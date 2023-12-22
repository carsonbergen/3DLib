using Godot;
using System;
using ThreeDLib;

public partial class orbit_camera : Node3D
{
	[ExportCategory("External nodes")]
	[Export]
	public Node3D target;
	[ExportCategory("Camera properties")]
	[Export]
	public Camera3D camera;
	[Export]
	public float movementSpeed = 15f;
	[Export]
	public float cameraSensitivity = 16f / 1000f;
	[Export]
	public float cameraAccelerationSpeed = 0.5f;
	[Export]
	public float cameraAccelerationAmount = 0.5f;
	[Export]
	public float baseCameraUpdateSpeed = 0.5f;
	[Export]
	public float fastCameraUpdateSpeed = 0.75f;

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

		var childCamera = GetNode<Camera3D>("Camera3D");
		if (childCamera != null || camera != null)
		{
			camera = childCamera;
			camera.Current = true;
		}
		else
		{
			handleError("Orbit camera requires camera node");
		}

		if (target == null)
		{
			handleError("Target node cannot be null");
		}
	}

	public override void _Process(double delta)
	{
		GlobalPosition = GlobalPosition.Lerp(target.Position, (float)delta * movementSpeed);
		var look = Input.GetAxis("camera_rotate_left", "camera_rotate_right");
		if (time > cameraAccelerationSpeed)
			internalCameraSensitivity = cameraSensitivity + (cameraAccelerationAmount / 100f);
		if (look > 0)
		{
			time += delta;
			RotateY(Mathf.DegToRad(internalCameraSensitivity));
		}
		else if (look < 0)
		{
			time += delta;
			RotateY(Mathf.DegToRad(-internalCameraSensitivity));
		}
		else
		{
			internalCameraSensitivity = cameraSensitivity;
			time = 0;
		}
		Mathf.Clamp(internalCameraSensitivity, cameraSensitivity, maxInternalCameraSensitivity);
	}

	private void handleError(string message)
	{
		GD.PrintErr(message);
		GetTree().Quit();
	}
}
