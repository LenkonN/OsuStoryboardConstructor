using Godot;
using System;

public partial class SystemMessageLabel : Label
{
	[Export] private Godot.Timer _killTimer;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	public void InitLabel(string text)
	{
		this.Text = text;
		_killTimer.WaitTime = 2 + (text.Length * 0.1);
		_killTimer.Start();

	}

	private void OnTimeout()
	{
		QueueFree();
	}
}
