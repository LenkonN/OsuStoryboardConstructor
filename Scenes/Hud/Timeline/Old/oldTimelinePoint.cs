using Godot;
using System;

public partial class oldTimelinePoint : PanelContainer
{
    [Export] private float _visualOffset = 0;

    public override void _Ready()
	{
		Position = new Vector2(GlobalPosition.X + _visualOffset, GlobalPosition.Y);
	}


	public override void _Process(double delta)
	{

	}
}
