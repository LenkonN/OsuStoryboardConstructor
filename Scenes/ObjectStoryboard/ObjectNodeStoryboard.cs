using Godot;
using Godot.NativeInterop;
using System;

/// <summary>
/// Main class for all storyboard node objects of the program. All values are equated to the osu! format.
/// Also, these values are used to display to the user and they may not be equal to the node values (i.e. GlobalPosition is not equal to PositionStoryboard).
/// </summary>
public abstract partial class ObjectNodeStoryboard : AnimatedSprite2D
{
    public DataObject DataObject { get; protected set; }
    public Vector2 PositionStoryboard
	{
		get
		{
			return _position;
        }

		set
		{
            _position = value;
            ((DataAttributes.Sprite)DataObject.Attributes).Position = new float[2] { value.X, value.Y };
            this.GlobalPosition = _position;

        }
    }

    public float RotateStoryboard 
	{
		get 
		{
			return _rotate;
		}
		set
		{
			_rotate = value;
			((DataAttributes.Sprite)DataObject.Attributes).Rotate = _rotate;
            this.Rotation = _rotate * (2 * Mathf.Pi);
        }
	}
    public Vector2 ScaleStoryboard
	{
		get
		{
			return _scale;
		}

		set
		{
			_scale = value;
			((DataAttributes.Sprite)DataObject.Attributes).Scale = new float[2] { value.X, value.Y };
            this.Scale = _scale;
		}
	}

	private Vector2 _position;
    private float _rotate;
	private Vector2 _scale;

    public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

    public virtual void LoadDataObject(DataObject dataObject)
    {
        DataObject = dataObject;

		PositionStoryboard = new Vector2(
			((DataAttributes.Sprite)dataObject.Attributes).Position[(int)Vector2Json.X],
			((DataAttributes.Sprite)dataObject.Attributes).Position[(int)Vector2Json.Y]
			);

		RotateStoryboard = ((DataAttributes.Sprite)dataObject.Attributes).Rotate;

		ScaleStoryboard = new Vector2(
            ((DataAttributes.Sprite)dataObject.Attributes).Scale[(int)Vector2Json.X],
            ((DataAttributes.Sprite)dataObject.Attributes).Scale[(int)Vector2Json.Y]
            );
    }

    protected void ChangePosToOsuFormat()
	{
		//to-do
	}

}
