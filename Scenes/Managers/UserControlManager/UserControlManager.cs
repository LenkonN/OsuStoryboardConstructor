using Godot;
using System;

public partial class UserControlManager : Node
{
	[Export] public float RotateSensivity = 0.01f;
	[Export] public float ScaleSensivity = 0.01f;

	public event Action<UserControl> UserControlChangeEvent;
	public static UserControlManager Instance { get; set; }
	public UserControl UserControl { get; set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{

    }

	public void SetControlToMove()
	{
		UserControl = UserControl.Move;
        UserControlChangeEvent?.Invoke(UserControl);
	}
	public void SetControlToRotate()
	{
		UserControl = UserControl.Rotate;
        UserControlChangeEvent?.Invoke(UserControl);
    }

	public void SetControlToScale()
	{
		UserControl = UserControl.Scale;
        UserControlChangeEvent?.Invoke(UserControl);
    }

}
