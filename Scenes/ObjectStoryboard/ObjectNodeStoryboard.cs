using Godot;
using Godot.NativeInterop;
using System;

/// <summary>
/// Main class for all storyboard node objects of the program. All values are equated to the osu! format.
/// Also, these values are used to display to the user and they may not be equal to the node values (i.e. GlobalPosition is not equal to PositionStoryboard).
/// </summary>
public abstract partial class ObjectNodeStoryboard : AnimatedSprite2D
{
	[Export] private CollisionShape2D _collision;
	[Export] private Panel _panelBorder;

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

	public string ImagePath { get; set; }
	
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

		ImagePath = ((DataAttributes.Sprite)dataObject.Attributes).ImagePath;

		LoadImage();
    }

	private void LoadImage()
	{
		if (this.SpriteFrames.GetFrameCount("Based") != 0)
		{
			for (int i = 0; i < this.SpriteFrames.GetFrameCount("Based"); i++)
			{
				this.SpriteFrames.RemoveFrame("Based", i);
			}
		}

		Image image = new Image();
		Error error;

		if (FileAccess.FileExists(ImagePath))
		{
			error = image.Load(ImagePath);
		}

		else
		{
			error = image.Load($"{Environment.Instance.FolderImageLibrary}/Program/Not_found.png");
		}

		if (error == Error.Ok)
		{
			ImageTexture texture = ImageTexture.CreateFromImage(image);
			this.SpriteFrames.AddFrame("Based", texture);
			Vector2 imageSize = new Vector2(image.GetSize().X, image.GetSize().Y);

            RectangleShape2D rectangleShape = _collision.Shape as RectangleShape2D;
			rectangleShape.Size = imageSize;

			_panelBorder.Size = imageSize;
			_panelBorder.Position = -(imageSize / 2);
        }
	}

	public virtual void QueueFreeObject()
	{
		QueueFree();
	}

    protected void ChangePosToOsuFormat()
	{
		//to-do
	}

}
