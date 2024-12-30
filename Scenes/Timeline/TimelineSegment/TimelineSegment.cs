using Godot;
using System;

public partial class TimelineSegment : Control
{

	public int SegmentIndex { get; private set; }
	public OsuDataTime OsuDataTime { get; private set; }

	[Export] private Label _indexLabel;
    [Export] private Label _timeLabel;

    [Export] private AnimationPlayer _animation;
	public override void _Ready()
	{
		ExportManager.Instance.ToEditor.FinishedImportJsonEvent += CountTime;
	}

	public override void _Process(double delta)
	{
		VisualSelect();
    }

	public void SetSegment(int index)
	{
		SegmentIndex = index;

		_indexLabel.Text = "[ " + index + " ]";

		if (Timeline.Instance.DefaultRightCountSegment < index)
			CountTime();
    }

	private void CountTime()
	{

        OsuDataTime = new OsuDataTime()
		{
			Mil = (int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset,
			Sec = ((int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset) * 1000,
			Min = (((int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset) * 1000) * 60
        };

        _timeLabel.Text = "[" + OsuDataTime.Mil + "ms]";
    }

	private void VisualSelect()
	{
		if (Timeline.Instance.CurrentSegmentSelected == SegmentIndex)
		{
			_animation.Play("Select");
		}

		else if (Timeline.Instance.CurrentSegmentSelected != SegmentIndex)
		{
			_animation.Play("NotSelect");
		}
	}
}
