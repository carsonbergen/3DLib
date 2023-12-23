using Godot;
using System;

public partial class CommandHandler : Node
{
	[Signal]
	public delegate void CommandNotRecognizedEventHandler();
	[Signal]
	public delegate void CommandResponseEventHandler(string response);
	private string response;

	public string CurrentCommand(Array arguments) 
	{
		return "[color=red]Needs more arguments.[/color]";
	}

	private void OnCommandEnteredSignal(string command) 
	{
		var commandArray = command.Split(" ");
		GD.Print(commandArray);
		switch (commandArray[0])
		{
			case "current":
				response = CurrentCommand(commandArray);
				EmitSignal(SignalName.CommandResponse, response);
				break;
			default:
				EmitSignal(SignalName.CommandNotRecognized);
				break;
		}
	}
}
