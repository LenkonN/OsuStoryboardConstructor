using Godot;
using System;

public partial class Main : Node
{

    public static Main Instance { get; private set; }	

	[Export] private PackedScene _editorScene;


	public override void _Ready()
	{
		Instance = this;
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
