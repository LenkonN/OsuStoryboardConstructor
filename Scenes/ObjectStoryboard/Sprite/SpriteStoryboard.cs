using Godot;
using System;
using System.Collections.Generic;

public partial class SpriteStoryboard : ObjectNodeStoryboard
{
	private bool _mouseHover;
	private bool _isDrag;
	private bool _isSelected;

	private bool _isMoveBlock;

	[Export] private Panel _panel;
	[Export] private Timer _blockTimer;

	public override void _Ready()
	{
		GetViewport().PhysicsObjectPickingSort = true;
        GetViewport().PhysicsObjectPickingFirstOnly = true;
        Editor.Instance.StoryboardNodeObjectManager.SelectNodeObjectEvent += OnAnySelectObject;
		
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

	private void OnAnySelectObject(ObjectNodeStoryboard objectNode)
	{
		if (objectNode != this)
			_isSelected = false;

		HighlightBorderToggle();
	}

	private void HighlightBorderToggle()
	{
		if (_isSelected)
			_panel.Visible = true;

		else if (!_isSelected)
			_panel.Visible = false;
    }

    private void OnSelected(Viewport viewport, InputEvent inputEvent, int shapeIdx)
	{
        if (inputEvent.IsActionPressed("Select"))
		{
			_isSelected = true;
            BlockMove();
            Editor.Instance.StoryboardNodeObjectManager.OnSelectObject(this);
		}
    }

	private void BlockMove()
	{
        _isMoveBlock = true;
		_blockTimer.Start();
    }

	private void OnDrag()
	{
        if (Input.IsActionPressed("Select") && _mouseHover && _isSelected && !_isMoveBlock)
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
			this.PositionStoryboard = this.GlobalPosition;
			Editor.Instance.Hud.TreeParametres.OnSpriteEditorMove(this);
        }
	}

	private void OnMouseExited(int idx)
	{
		_mouseHover = false;
    }

	private void OnMouseEntered(int idx)
	{
		_mouseHover = true;
    }

	private void MoveBlockTimeout()
	{
		_isMoveBlock = false;
	}
}
