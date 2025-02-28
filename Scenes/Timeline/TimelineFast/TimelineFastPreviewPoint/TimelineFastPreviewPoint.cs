using Godot;
using System;

public partial class TimelineFastPreviewPoint : Panel
{
	[Export] private Label _indexLabel;
    [Export] private Label _timeLabel;
    [Export] private Label _keyLabel;

    [Export] private PackedScene _segmentScene { get; set; }

    public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}
    private TimelineSegment CreateTheoreticalSegment(int index)
    {
        TimelineSegment segment = _segmentScene.Instantiate<TimelineSegment>();
        segment.SetSegment(index);
        return segment;
    }

    public void ReqSetText(int targetIndex)
	{

        TimelineSegment segment = CreateTheoreticalSegment(targetIndex);
        DataTimelineSegment dataSegment = segment.DataSegment;

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

        string percent = Math.Round((Convert.ToSingle(targetIndex) / Convert.ToSingle(TimelineCore.Instance.GetCountVirtualSegmentsRight()) * 100)).ToString();

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
