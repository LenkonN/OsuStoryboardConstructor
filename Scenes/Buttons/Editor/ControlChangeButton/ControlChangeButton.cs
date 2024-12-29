using Godot;
using System;

public partial class ControlChangeButton : TextureRect
{

	[Export] private UserControl _buttonType { get; set; }
	[Export] private AnimationPlayer _animation { get; set; }
	[Export] private Button _button { get; set; }
	public override void _Ready()
	{
		UserControlManager.Instance.UserControlChangeEvent += OnControlChanged;

    }

	public override void _Process(double delta)
	{

	}

	private void OnControlChanged(UserControl userControl)
	{
		_animation.Play("False");

		if (_buttonType == userControl)
		{
			_animation.Play("True");
		}

    }

	private void OnButton()
	{
		if(_buttonType is UserControl.Move)
			UserControlManager.Instance.SetControlToMove();

        else if (_buttonType is UserControl.Rotate)
            UserControlManager.Instance.SetControlToRotate();

        else if (_buttonType is UserControl.Scale)
            UserControlManager.Instance.SetControlToScale();
    }
}
