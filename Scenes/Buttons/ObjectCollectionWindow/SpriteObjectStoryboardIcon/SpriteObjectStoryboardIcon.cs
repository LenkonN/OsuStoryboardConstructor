using Godot;
using System;

/// <summary>
/// Button icon in the object creation window.
/// </summary>
public partial class SpriteObjectStoryboardIcon : TextureRect
{
	[Export] private WindowObjectCollectionBox objectBox;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

    /// <summary>
    /// Set the selected object to "Sprite object".
    /// </summary>
    private void OnButton()
	{
		objectBox.Window.SelectItem(ObjectsTypeList.Sprite);
	}
}
