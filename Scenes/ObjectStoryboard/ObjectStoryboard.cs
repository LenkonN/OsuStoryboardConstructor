using Godot;
using System;

/// <summary>
/// Main class for all storyboard objects of the program. All values are equated to the osu! format.
/// Also, these values are used to display to the user and they may not be equal to the node values (i.e. GlobalPosition is not equal to PositionStoryboard).
/// </summary>
public abstract partial class ObjectStoryboard : AnimatedSprite2D
{
	protected Vector2 _positionStoryboard
	{
		get
		{
			return _position;
		}

		set
		{
			_position = value;
			this.GlobalPosition = _position;
		}
	}
	protected float _rotateStoryboard 
	{
		get 
		{
			return _rotate;
		}
		set
		{
			_rotate = value;
            this.GlobalRotation = _rotate;
        }
	}
	protected Vector2 _scaleStoryboard
	{
		get
		{
			return _scale;
		}

		set
		{
			_scale = value;
			this.GlobalScale = _scale;
		}
	}
	protected ObjectsTypeList _objectType;

	private Vector2 _position;
    private float _rotate;
	private Vector2 _scale;

    public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	protected void ChangePosToOsuFormat()
	{
		//to-do
	}
}
