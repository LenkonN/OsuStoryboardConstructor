using Godot;
using System;

public partial class AddObjectMenuButton : TextureRect
{

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	private void OnClick()
	{
		Editor.Instance.CreateObjectCollectionWindow();
	}
}
