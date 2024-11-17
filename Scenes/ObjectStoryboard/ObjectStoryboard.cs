using Godot;
using System;

/// <summary>
/// Main class for all storyboard objects of the program. All values are equated to the osu! format.
/// Also, these values are used to display to the user and they may not be equal to the node values (i.e. GlobalPosition is not equal to PositionStoryboard).
/// </summary>
public abstract partial class ObjectStoryboard : AnimatedSprite2D
{
	protected Vector2 _positionStoryboard;
	protected float _rotateStoryboard 
	{
		get 
		{
			return _rotate;
		}
		set
		{
			this.GlobalRotation = value;
			_rotate = value;
		}
	}
	private float _rotate;
	protected float _scaleStoryboard;
	protected ObjectsTypeList _objectType;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}
}
