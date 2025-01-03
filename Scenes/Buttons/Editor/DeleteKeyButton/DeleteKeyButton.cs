using Godot;
using System;

public partial class DeleteKeyButton : TextureRect
{

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

    private void OnButton()
    {
        TimelineSegment timelineSegment = Timeline.Instance.CurrentSegmentSelected;
		timelineSegment.ReqDeleteKey();
    }
}
