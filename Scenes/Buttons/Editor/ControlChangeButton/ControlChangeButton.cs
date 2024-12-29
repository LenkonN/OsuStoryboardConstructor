using Godot;
using System;

/// <summary>
/// Menu button in editor. Need for user selects which operation he wants to do (Move, Rotate, Scale).
/// </summary>
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
        CheckKeyForChangeControl();
    }


    /// <summary>
    /// Switching the operation by pressing hotkeys.
    /// </summary>
    private void CheckKeyForChangeControl()
    {
        if (Input.IsActionJustPressed("Move") && _buttonType is UserControl.Move)
        {
            _button.ButtonPressed = true;
            _button.EmitSignal("pressed");
            _button.GrabFocus();
        }

        else if (Input.IsActionJustPressed("Rotate") && _buttonType is UserControl.Rotate)
        {
            _button.ButtonPressed = true;
            _button.EmitSignal("pressed");
            _button.GrabFocus();
        }

        else if (Input.IsActionJustPressed("Scale") && _buttonType is UserControl.Scale)
        {
            _button.ButtonPressed = true;
            _button.EmitSignal("pressed");
            _button.GrabFocus();
        }
    }

    /// <summary>
    /// Highlighting button.
    /// </summary>
    /// <param name="userControl">Button type.</param>
    private void OnControlChanged(UserControl userControl)
	{
		_animation.Play("False");

		if (_buttonType == userControl)
		{
			_animation.Play("True");
		}

    }

    /// <summary>
    /// Event on click button. Causes the user to change the mode of interaction with an object
    /// </summary>
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
