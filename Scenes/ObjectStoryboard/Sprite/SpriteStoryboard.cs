using Godot;
using System;
using System.Collections.Generic;

public partial class SpriteStoryboard : ObjectStoryboard
{
	private DataObject _dataObject;
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	public void LoadDataObject(DataObject dataObject)
	{
		_dataObject = dataObject;
	}

	public DataObject CreateDataObject(Vector2 pos, float rotate, Vector2 scale)
	{
		_dataObject = new DataObject()
		{
			UID = Editor.Instance.StoryboardObjectStructureManager.GenerateUID(),
			Name = "New sprite",
			Description = "",
			ObjectType = ObjectsTypeList.Sprite,
			Items = new List<KeyValuePair<string, DataObject>>(),
			Attributes = new DataAttributes.Sprite()
			{
				Position = pos,
				Rotate = rotate,
				Scale = scale,
				ImagePath = ""
			}
		};

		return _dataObject;
	}

	public void SetObjectValues()
	{
		Console.WriteLine(((DataAttributes.Sprite)_dataObject.Attributes).Position);

		_positionStoryboard = ((DataAttributes.Sprite)_dataObject.Attributes).Position;
		_rotateStoryboard = ((DataAttributes.Sprite)_dataObject.Attributes).Rotate;
        _scaleStoryboard = ((DataAttributes.Sprite)_dataObject.Attributes).Scale;
    }
}
