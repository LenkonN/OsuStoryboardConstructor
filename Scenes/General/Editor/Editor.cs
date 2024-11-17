using Godot;
using System;

public partial class Editor : Node2D
{
    [Export] private PackedScene _ObjectCollectionWindowScene;

	public static Editor Instance { get; set; }

    public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{
		
	}

	public void CreateObjectCollectionWindow()
	{
        AddChild(_ObjectCollectionWindowScene.Instantiate<ObjectCollectionWindow>());
    }
}
