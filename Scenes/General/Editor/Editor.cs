using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Editor : Node2D
{
    [Export] private PackedScene _ObjectCollectionWindowScene;

	[Export] public StoryboardObjectStructureManager StoryboardObjectStructureManager;

    public List<DataObject> StoryboardObjectList = new List<DataObject>();

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
