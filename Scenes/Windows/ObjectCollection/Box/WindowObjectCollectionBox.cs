using Godot;
using System;

public partial class WindowObjectCollectionBox : MarginContainer
{
	[Export] public ObjectCollectionWindow Window;
	[Export] public Label ObjectNameLabel;

	public override void _Ready()
	{

	}

	
	public override void _Process(double delta)
	{

	}

	private void OnButton()
	{
		Window.OnCreateButton();
    }
}
