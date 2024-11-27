using Godot;
using System;
using System.Collections.Generic;

public partial class SpriteStoryboard : ObjectNodeStoryboard
{
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	public DataObject CreateDataObject(Vector2 pos, float rotate, Vector2 scale)
	{
		DataObject = new DataObject()
		{
			UID = Editor.Instance.StoryboardObjectStructureManager.GenerateUID(),
			Name = "New sprite",
			Description = "",
			ObjectType = ObjectsTypeList.Sprite,
			Items = new List<KeyValuePair<string, DataObject>>(),
			Attributes = new DataAttributes.Sprite()
			{
				Position = new float[2] { pos.X, pos.Y},
				Rotate = rotate,
				Scale = new float[2] { scale.X, scale.Y },
				ImagePath = ""
			}
		};

        return DataObject;
	}
}
