using Godot;
using System;

public partial class GameEventHandler : Node
{
	[Export]
	public Label messageLabel;

	public void PlayerInRangeOfInteractableObject(Node3D node)
	{
		if (messageLabel != null)
		{
			messageLabel.Text = "Player in range of " + node.Name;
		}
	}

	public void PlayerOutOfRangeOfInteractableObject(Node3D node)
	{
		if (messageLabel != null)
		{
			messageLabel.Text = "Player out of range of " + node.Name;
		}
	}
}
