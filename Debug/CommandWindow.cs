using Godot;
using System;

public partial class CommandWindow : Window
{
	[ExportCategory("Internal nodes")]
	[Export]
	public LineEdit commandInput;
	[Export]
	public RichTextLabel commandDisplay;

	/** 
		Command entered signal
			Sent to CommandHandler
	*/
	[Signal]
	public delegate void CommandEnteredSignalEventHandler(string command);
	private string command;

	public override void _Ready()
	{
		if (commandInput == null)
		{
			commandInput = GetNode<LineEdit>("CommandInput");
			if (commandInput == null)
			{
				GD.PrintErr("CommandInput cannot be null");
				GetTree().Quit();
			}
		}

		if (commandDisplay == null)
		{
			commandDisplay = GetNode<RichTextLabel>("CommandDisplay");
			if (commandDisplay == null)
			{
				GD.PrintErr("CommandDisplay cannot be null");
				GetTree().Quit();
			}
		}
	}

	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        switch (@event)
		{
			case InputEventKey:
				/**
					Command entered
				*/
				if (Input.IsActionJustPressed("ui_accept"))
				{
					command = commandInput.Text;
					PushToDisplay("[color=yellow]$ [/color]	" + command);
					commandInput.Clear();
					EmitSignal(SignalName.CommandEnteredSignal, command);
				}
				break;
		}
    }

	/**
		Pushes normal text to the display
	*/
	public void PushToDisplay(string text)
	{
		commandDisplay.Text += text + "\n";
	}

	/**
		Pushes error text to the display
	*/
	public void PushErrorToDisplay(string text)
	{
		commandDisplay.Text += "[color=red]" + text + "[/color]\n";
	}

	/**
		Handles CommandNotRecognized from the CommandHandler class
	*/
	private void OnCommandNotRecognized() 
	{
		PushErrorToDisplay("Command not recognized");
	}

	/**
		Handles CommandResponse from the CommandHandler class
	*/
	private void OnCommandResponse(string response) 
	{
		PushToDisplay(response);
	}
}
