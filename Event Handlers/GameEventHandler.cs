using Godot;
using System;

public partial class GameEventHandler : Node
{
	[Export]
	public Label interactionLabel;

	public string GetInteractionText(string nodeName = "", string interactionMessage = "")
	{
		// Get the first action 
		InputEvent action = InputMap.ActionGetEvents("interact")[0];
		return "Press " + action.AsText()[0] + " " + interactionMessage + nodeName;
	}

	public void PlayerInRangeOfInteractableObject(Node3D node)
	{
		if (interactionLabel != null)
		{
			interactionLabel.Text = GetInteractionText((string)node.GetMeta("interaction_message"));
		}
	}

	public void PlayerOutOfRangeOfInteractableObject(Node3D node)
	{
		if (interactionLabel != null)
		{
			interactionLabel.Text = "";
		}
	}

	public enum GameEvent
	{
		PlayerInRangeOfInteractable,
	}

	
}
