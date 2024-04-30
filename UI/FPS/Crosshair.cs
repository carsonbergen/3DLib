using Godot;
using System;

public partial class Crosshair : CenterContainer
{
	[Export]
	public Line2D leftLine;
	[Export]
	public Line2D rightLine;
	[Export]
	public Line2D topLine;
	[Export]
	public Line2D bottomLine;

	[Export]
	public float spread = 5;
	[Export]
	public float length = 10;

	public override void _Ready()
	{
		adjustSpread(spread);
		adjustLength(length);
	}

	public void adjustSpread(float newSpread)
	{
		spread = newSpread;
		// Set up left line
		leftLine.SetPointPosition(0, new Vector2(-newSpread, 0));
		leftLine.SetPointPosition(1, new Vector2(-(newSpread + length), 0));
		// Set up right line
		rightLine.SetPointPosition(0, new Vector2(newSpread, 0));
		rightLine.SetPointPosition(1, new Vector2(newSpread + length, 0));
		// Set up top line
		topLine.SetPointPosition(0, new Vector2(0, -newSpread));
		topLine.SetPointPosition(1, new Vector2(0, -(newSpread + length)));
		// Set up bottom line
		bottomLine.SetPointPosition(0, new Vector2(0, newSpread));
		bottomLine.SetPointPosition(1, new Vector2(0, newSpread + length));
	}

	public void adjustLength(float newLength)
	{
		length = newLength;
		// Set up left line
		leftLine.SetPointPosition(0, new Vector2(-spread, 0));
		leftLine.SetPointPosition(1, new Vector2(-(spread + newLength), 0));
		// Set up right line
		rightLine.SetPointPosition(0, new Vector2(spread, 0));
		rightLine.SetPointPosition(1, new Vector2(spread + newLength, 0));
		// Set up top line
		topLine.SetPointPosition(0, new Vector2(0, -spread));
		topLine.SetPointPosition(1, new Vector2(0, -(spread + newLength)));
		// Set up bottom line
		bottomLine.SetPointPosition(0, new Vector2(0, spread));
		bottomLine.SetPointPosition(1, new Vector2(0, spread + newLength));
	}
}
