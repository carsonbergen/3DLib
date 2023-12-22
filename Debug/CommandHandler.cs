using Godot;
using System;

public partial class CommandHandler : Node
{
	[Signal]
	public delegate void CommandNotRecognizedEventHandler();
	[Signal]
	public delegate void CommandResponseEventHandler(string response);
	private string response;


	private void OnCommandEnteredSignal(string command) 
	{
		
	}
}
