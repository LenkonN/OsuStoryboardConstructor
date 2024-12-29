using Godot;
using System;

public partial class SystemMessageManager : CanvasLayer
{
	public static SystemMessageManager Instance {  get; set; }

    [Export] private PackedScene _labelMessageScene;
    [Export] private VBoxContainer _contentBox;
	[Export] private UpdateMessage _updateMessage;

	public int LastTimeStartUpdateProject;
	public int LastTimeEndUpdateProject;

	public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{
		ChangeVisible();
	}

	private void ChangeVisible()
	{
		if (_contentBox.GetChildCount() == 0)
			this.Visible = false;
		
		else
			this.Visible = true;
	}

	public void CreateMessage(string text)
	{
		SystemMessageLabel label = _labelMessageScene.Instantiate<SystemMessageLabel>();
        _contentBox.AddChild(label);
        label.InitLabel(text);

    }
}
