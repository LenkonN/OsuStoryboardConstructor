using Godot;
using System;

public partial class CreateKeyButton : TextureRect
{
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	private void OnButton()
	{
		TimelineSegment timelineSegment = TimelineCore.Instance.CurrentSegmentSelected;
		timelineSegment.ReqCreateKey();
	}
}
