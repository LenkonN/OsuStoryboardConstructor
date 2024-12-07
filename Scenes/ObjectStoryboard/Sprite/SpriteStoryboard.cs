using Godot;
using System;
using System.Collections.Generic;

public partial class SpriteStoryboard : ObjectNodeStoryboard
{
	private bool _mouseHover;
	private bool _isDrag;

	public override void _Ready()
	{
		GetViewport().PhysicsObjectPickingSort = true;
        GetViewport().PhysicsObjectPickingFirstOnly = true;
    }

	public override void _Process(double delta)
	{
		OnDrag();
		OnDrop();
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

	private void OnSelected(Node viewport, InputEvent inputEvent, int shapeIdx)
	{
		if (inputEvent.IsActionPressed("Select"))
		{
			Editor.Instance.Hud.TreeObjects.SelectObjectByDataObject(DataObject);
		}

    }

	private void OnDrag()
	{
        if (Input.IsActionPressed("Select") && _mouseHover)
        {
            this.GlobalPosition = GetGlobalMousePosition();
			_isDrag = true;
        }
    }

	private void OnDrop()
	{
		if (Input.IsActionJustReleased("Select") && _isDrag)
		{
			_isDrag = false;
			this.PositionStoryboard = GetGlobalMousePosition();
			Editor.Instance.Hud.TreeParametres.OnSpriteEditorMove(this);
        }
	}

	private void OnMouseExit()
	{
		_mouseHover = false;
	}

	private void OnMouseEntered()
	{
		_mouseHover = true;
	}
}
