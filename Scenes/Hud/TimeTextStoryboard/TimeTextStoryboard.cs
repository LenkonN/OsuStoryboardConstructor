using Godot;
using System;
using System.Threading.Tasks;

public partial class TimeTextStoryboard : Label
{
	public override void _Ready()
	{
		Init();
	}


	public override void _Process(double delta)
	{

	}

	async private void Init()
	{
		await Task.Delay(2500);
        TimelineCore.Instance.SelectedSegmentChangedEvent += SetTime;
		SetTime();
    }

	private void SetTime()
	{
		TimelineSegment time = TimelineCore.Instance.CurrentSegmentSelected;

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
