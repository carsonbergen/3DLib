using Godot;
using System;
using System.Linq;

public partial class CommandHandler : Node
{
	[Signal]
	public delegate void CommandNotRecognizedEventHandler();
	[Signal]
	public delegate void CommandResponseEventHandler(string response);
	private string response;

	private string EncodeError(string str)
	{
		return "[color=red]" + str + "[/color]";
	}

	private string CommandErrorChecking(int minArgs, int maxArgs, int args)
	{
		if (args > maxArgs)
			return EncodeError("Too many arguments");
		else if (args < minArgs)
			return EncodeError("Needs more arguments");
		else
			return null;
	}

	public string CurrentCommand(Array arguments)
	{
		var err = CommandErrorChecking(1, 1, arguments.Length);
		if (err != null)
			return err;
		
		return EncodeError("Arguments not recognized");
	}

	private void OnCommandEnteredSignal(string command)
	{
		var commandArray = command.Split(" ");
		var arguments = commandArray.Skip(1).ToArray();

		switch (commandArray[0])
		{
			case "current":
				response = CurrentCommand(arguments);
				EmitSignal(SignalName.CommandResponse, response);
				break;
			default:
				EmitSignal(SignalName.CommandNotRecognized);
				break;
		}
	}
}
