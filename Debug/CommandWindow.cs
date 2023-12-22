using Godot;
using System;

public partial class CommandWindow : Window
{
	[ExportCategory("Internal nodes")]
	[Export]
	public LineEdit commandInput;
	[Export]
	public RichTextLabel commandDisplay;

	// Signal stuff
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
				if (Input.IsActionJustPressed("ui_accept"))
				{
					command = commandInput.Text;
					commandDisplay.Text += "[color=yellow]$ [/color]	" + command + "\n";
					commandInput.Clear();
					EmitSignal(SignalName.CommandEnteredSignal, command);
				}
				break;
		}
    }

	private void OnCommandNotRecognized() 
	{
		commandDisplay.Text += "[color=red]Command not recognized.[/color]";
	}
}
