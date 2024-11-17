using Godot;
using System;

public partial class Main : Node
{

	[Export] private PackedScene _editorScene;

	public override void _Ready()
	{
		CreateEditor();
	}

	public override void _Process(double delta)
	{

	}

	public void CreateEditor()
	{
		AddChild(_editorScene.Instantiate<Editor>());
	}
}
