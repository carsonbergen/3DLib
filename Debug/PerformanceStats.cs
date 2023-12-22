using Godot;
using System;
using System.Threading;

public partial class PerformanceStats : RichTextLabel
{
	private static string updateFromStatThread;
	private static readonly object lockObject = new object();
	private static readonly AutoResetEvent childThreadSignal = new AutoResetEvent(false);

	private Thread statThread;

	public override void _Ready()
	{
		// Start stat thread
		statThread = new Thread(StatThread);
		statThread.Start();
	}

	public override void _Process(double delta)
	{
		var newText = GetUpdateFromStatThread();
		if (newText != Text)
		{
			Text = newText;
		}
	}


	// Get information from the Godot engine, format it, and store it in "text"
	public static void StatThread()
	{
		string formatTitle(string str)
		{
			return "[color=yellow][u][b]" + str + "[/b][/u][/color]\n";
		}

		string formatSubtext(string str)
		{
			return "[indent][font_size=14]" + str + "[/font_size][/indent]\n";
		}

		string formatProperty(string propName, string propData, string hint = "")
		{
			if (hint != null)
			{
				return formatSubtext(
					"[hint=" + hint + "]" +
					"[b]" + propName + "[/b]:" +
					"[i]" + propData + "[/i]" +
					"[/hint]"
				);
			}
			return formatSubtext(
				"[b]" + propName + "[/b]:" +
				"[i]" + propData + "[/i]"
			);
		}

		try
		{
			while (true)
			{
				var information = "";
				information += formatTitle("SYSTEM INFORMATION:");

				var arch = Engine.GetArchitectureName();
				information += formatProperty("CPU ARCHITECTURE", arch, "CPU architecture the Godot binary was built for.");
				information += formatProperty("Engine Version", Engine.GetVersionInfo()["string"].ToString(), "Current engine version.");

				information += formatTitle("PERFORMANCE INFORMATION:");

				// FPS
				var fps = formatProperty("FPS", Engine.GetFramesPerSecond().ToString(), "Current FPS.");
				information += fps;

				information += formatTitle("GAME INFORMATION:");

				var vid_mem = formatProperty("Video Memory Usage", Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed).ToString(), "Video memory used. Lower is better.");
				information += vid_mem;

				Thread.Sleep(10);

				lock (lockObject)
				{
					updateFromStatThread = information;
				}

				childThreadSignal.Set();
			}
		}
		catch (ThreadAbortException)
		{
			GD.PrintErr("Stat Thread aborted");
		}
	}

	public string GetUpdateFromStatThread()
	{
		lock (lockObject)
		{
			string update = updateFromStatThread;
			updateFromStatThread = null;
			return update ?? Text;
		}
	}
}
