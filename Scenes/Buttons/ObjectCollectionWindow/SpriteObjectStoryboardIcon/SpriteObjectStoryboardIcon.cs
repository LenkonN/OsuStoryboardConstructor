using Godot;
using System;

public partial class SpriteObjectStoryboardIcon : TextureRect
{
	[Export] private WindowObjectCollectionBox objectBox;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	private void OnButton()
	{
		objectBox.Window.SelectItem(ObjectsTypeList.Sprite);
	}
}
