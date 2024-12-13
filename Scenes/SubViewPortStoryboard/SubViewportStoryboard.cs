using Godot;
using System;

public partial class SubViewportStoryboard : SubViewport
{

	public override void _Ready()
	{
        SetProcessInput(true);
	}

	public override void _Process(double delta)
	{

	}
}
