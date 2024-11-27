using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Editor : Node2D
{
	[Export] public AnimationPlayer StoryboardPlayer;

	[Export] private PackedScene _ObjectCollectionWindowScene;

	[Export] public StoryboardObjectStructureManager StoryboardObjectStructureManager;
	[Export] public StoryboardNodeObjectManager StoryboardNodeObjectManager;
	[Export] public CanvasLayer StoryboardCanvasLayer;
	[Export] public Hud Hud;

	public List<DataObject> StoryboardObjectList = new List<DataObject>();
	public List<ObjectNodeStoryboard> StoryboardNodeList = new List<ObjectNodeStoryboard>();

	public bool IsFirstLoad { get; private set; } = true;

	public static Editor Instance { get; set; }

    public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{
		
	}

	public void LockFlagFirstUpdate()
	{
		IsFirstLoad = false;
	}

	public void CreateObjectCollectionWindow()
	{
        AddChild(_ObjectCollectionWindowScene.Instantiate<ObjectCollectionWindow>());
    }

	private void OnTreeExit()
	{
		ExportManager.Instance.Json.SaveJsonFileWithoutEvents(StoryboardObjectStructureManager.StoryboardStructureData);
	}
}
