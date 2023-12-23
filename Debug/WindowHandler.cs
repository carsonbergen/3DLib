using Godot;
using Godot.Collections;
using System;

public partial class WindowHandler : Node
{
	[Export]
	public Array<Window> windows;

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("open_command_terminal"))
		{
			foreach (Window window in windows)
			{
				window.Visible = !window.Visible;
			}
		}
	}
}
