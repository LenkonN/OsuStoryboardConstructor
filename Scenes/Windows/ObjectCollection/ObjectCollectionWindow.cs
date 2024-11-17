using Godot;
using System;

public partial class ObjectCollectionWindow : Window
{
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	private void OnClose()
	{
		QueueFree();
	}
}
