using Godot;
using System;

public partial class NewStoryboardObjectManager : Node
{

	[Export] private Marker2D _spawnPos;
	[Export] private PackedScene _spriteObjectScene;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	public void OnButtonNewObject(ObjectsTypeList objectType)
	{
		if (objectType is ObjectsTypeList.Sprite)
			CreateNewSprite(false);
	}

	public void CreateNewSprite(bool isLoad)
	{
		SpriteStoryboard spriteObject = _spriteObjectScene.Instantiate<SpriteStoryboard>();
        Editor.Instance.StoryboardCanvasLayer.AddChild(spriteObject);

		if (!isLoad)
		{
			DataObject dataObject = spriteObject.CreateDataObject(_spawnPos.GlobalPosition, _spawnPos.Rotation, _spawnPos.Scale);
			spriteObject.SetObjectValues();
			Console.WriteLine(dataObject.UID);
		}

	}
}
