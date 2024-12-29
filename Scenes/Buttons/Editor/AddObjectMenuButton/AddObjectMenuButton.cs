using Godot;
using System;

/// <summary>
/// Menu button in editor. Open window with all storyboard objects to be create.
/// </summary>
public partial class AddObjectMenuButton : TextureRect
{

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

    /// <summary>
    /// Event on click button. Called open window.
    /// </summary>
    private void OnClick()
	{
		Editor.Instance.CreateObjectCollectionWindow();
	}
}
