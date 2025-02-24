using Godot;
using System;
using System.Collections.Generic;

public partial class SpriteStoryboard : ObjectNodeStoryboard
{
	private bool _isMouseHover;
	private bool _isDrag;
	private bool _isSelected;
	private bool _isSelectBlock;

	private UserControl? _buffUserControl = null;
	private Vector2 _startInteractionPoint;

	private Vector2 _buffOldPosition;
	private float _buffOldRotation;
	private Vector2 _buffOldScale;

	[Export] private Panel _panel;
	[Export] private Timer _blockTimer;

	public override void _Ready()
	{
		GetViewport().PhysicsObjectPickingSort = true;
        GetViewport().PhysicsObjectPickingFirstOnly = true;

        Editor.Instance.StoryboardNodeObjectManager.SelectNodeObjectEvent += OnAnySelectObject;
		Editor.Instance.Hud.TreeObjects.SelectedItemEvent += OnAnySelectObject;

        UserControlManager.Instance.UserControlChangeEvent += OnDrop;


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
		{
			_isSelected = false;
		}

		else
		{
			_isSelected = true;
		}

		HighlightBorderToggle();
	}

	private void OnAnySelectObject(DataObjectTreeMetadata metadata)
	{
        ObjectNodeStoryboard objectNode = Editor.Instance.StoryboardNodeObjectManager.FindNodeObjectByMetadata(metadata);
		OnAnySelectObject(objectNode);
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
        _isSelectBlock = true;
		_blockTimer.Start();
    }

	private void OnDrag()
	{
		if (Input.IsActionPressed("Select") && _isSelected && !_isSelectBlock )
		{

			if (!_isDrag)
			{
				_startInteractionPoint = GetGlobalMousePosition();

                _buffOldPosition = this.PositionStoryboard;
                _buffOldRotation = this.RotateStoryboard;
				_buffOldScale = this.ScaleStoryboard;
            }

            _isDrag = true;
            _buffUserControl = UserControlManager.Instance.UserControl;

			if (UserControlManager.Instance.UserControl is UserControl.Move && _isMouseHover)
			{
				this.PositionStoryboard = GetGlobalMousePosition();
			}

			else if (UserControlManager.Instance.UserControl is UserControl.Rotate)
			{
				Vector2 different = _startInteractionPoint - GetGlobalMousePosition();
				float differentInt = different.X + different.Y;
				this.RotateStoryboard = differentInt * UserControlManager.Instance.RotateSensivity + _buffOldRotation;
			}

			else if (UserControlManager.Instance.UserControl is UserControl.Scale)
			{
                Vector2 different = _startInteractionPoint - GetGlobalMousePosition();
				this.ScaleStoryboard = different * UserControlManager.Instance.ScaleSensivity + _buffOldScale;	
            }
        }
    }

	private void OnDrop(UserControl userControl = UserControl.None)
	{
		if (Input.IsActionJustReleased("Select") && _isDrag)
		{
            Editor.Instance.Hud.TreeParametres.OnSpriteEditorInteraction(this);
            _isDrag = false;
           _buffUserControl = null;
        }
			
	}

    public override void QueueFreeObject()
    {
        Editor.Instance.StoryboardNodeObjectManager.SelectNodeObjectEvent -= OnAnySelectObject;
        Editor.Instance.Hud.TreeObjects.SelectedItemEvent -= OnAnySelectObject;
        UserControlManager.Instance.UserControlChangeEvent -= OnDrop;

        base.QueueFreeObject();
    }
    private void OnMouseExited(int idx)
	{
		_isMouseHover = false;
    }

	private void OnMouseEntered(int idx)
	{
		_isMouseHover = true;
    }

	private void SelectBlockTimeout()
	{
		_isSelectBlock = false;
	}
}
