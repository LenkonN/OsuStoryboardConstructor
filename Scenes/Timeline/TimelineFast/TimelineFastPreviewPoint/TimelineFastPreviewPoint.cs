using Godot;
using System;

public partial class TimelineFastPreviewPoint : Panel
{
	[Export] private Label _indexLabel;
    [Export] private Label _timeLabel;
    [Export] private Label _percentLabel;
    [Export] private Label _keyLabel;

    public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	public void ReqSetText(int targetIndex)
	{
        if (targetIndex > Timeline.Instance.DataSegmentList.Count - 1)
            targetIndex = Timeline.Instance.DataSegmentList.Count - 1;

        if (targetIndex < 0)
            targetIndex = 0;

        DataTimelineSegment dataSegment = Timeline.Instance.DataSegmentList[targetIndex];

        string time = null;

        if (dataSegment.OsuDataTime.Mil < 0)
            time = "-";
        else
            time = "";

        time +=
            $"{Math.Abs(dataSegment.OsuDataTime.Min).ToString("D2")}:" +
            $"{Math.Abs(dataSegment.OsuDataTime.Sec % 60).ToString("D2")}:" +
            $"{Math.Abs(dataSegment.OsuDataTime.Mil % 1000).ToString("D3")} " +
            $"({dataSegment.OsuDataTime.Mil})";


        string percent = null;

        if (targetIndex == Timeline.Instance.DataSegmentList.Count - 1)
            percent = "100";
        else
            percent = Math.Round((Convert.ToSingle(targetIndex) / Convert.ToSingle(Timeline.Instance.DataSegmentList.Count) * 100)).ToString();


        string animKey = null;

        if (dataSegment.DataKey != null)
            animKey = "Yes";
        else if (dataSegment.DataKey == null)
            animKey = "No";

        SetText(dataSegment.SegmentIndex.ToString(), time, percent, animKey);

    }

	private void SetText(string index, string time, string percent, string key)
	{
        _indexLabel.Text = "Index: " + index;
        _timeLabel.Text = $"Time: {time} ({percent}%)";
        _keyLabel.Text = "Key Animation: " + key;
    }
}
