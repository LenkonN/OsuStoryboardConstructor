using Godot;
using System;

public partial class TimeTextStoryboard : Label
{
	public override void _Ready()
	{
		Timeline.Instance.SelectedSegmentChangedEvent += SetTime;
	}


	public override void _Process(double delta)
	{

	}

	private void SetTime()
	{
		TimelineSegment time = Timeline.Instance.CurrentSegmentSelected;

		if (time.DataSegment.OsuDataTime.Mil < 0)
			this.Text = "-";
		else
			this.Text = "";

		this.Text +=
		$"{Math.Abs(time.DataSegment.OsuDataTime.Min).ToString("D2")}:" +
        $"{Math.Abs(time.DataSegment.OsuDataTime.Sec % 60).ToString("D2")}:" +
        $"{Math.Abs(time.DataSegment.OsuDataTime.Mil % 1000).ToString("D3")} " +
		$"({time.DataSegment.OsuDataTime.Mil})";
	}
}
