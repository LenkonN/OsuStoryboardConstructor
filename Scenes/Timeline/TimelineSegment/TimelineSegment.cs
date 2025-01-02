using Godot;
using System;

public partial class TimelineSegment : Control
{

	public int SegmentIndex { get; private set; }
	public OsuDataTime OsuDataTime { get; private set; }

	[Export] private Label _indexLabel;
    [Export] private Label _timeLabel;

    [Export] private AnimationPlayer _animationSelect;
	[Export] private AnimationPlayer _animationColor;
	private bool _isAnimationLock;
	public override void _Ready()
	{
		ExportManager.Instance.ToEditor.FinishedImportJsonEvent += CountTime;
		Timeline.Instance.SelectedSegmentChangedEvent += VisualSelect;
	}

	public override void _Process(double delta)
	{

    }

	public void SetSegment(int index)
	{
		SegmentIndex = index;
		this.Name = index.ToString();
		TickColor();

        _indexLabel.Text = "[ " + index + " ]";

		if (Timeline.Instance.DefaultRightCountSegment < index)
			CountTime();
    }

	private void CountTime()
	{

        OsuDataTime = new OsuDataTime()
		{
			Mil = (int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset,
			Sec = ((int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset) / 1000,
			Min = (((int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset) / 1000) / 60
        };

        _timeLabel.Text = OsuDataTime.Mil + "ms";
    }

	private void VisualSelect()
	{
		if (Timeline.Instance.CurrentSegmentIndexSelected == SegmentIndex)
		{
			_isAnimationLock = false;
			_animationSelect.Play("Select");
		}

		else if (Timeline.Instance.CurrentSegmentIndexSelected != SegmentIndex && !_isAnimationLock)
		{
			_isAnimationLock = true;	
			_animationSelect.Play("NotSelect");
		}
	}

	private void TickColor()
	{
		if (SegmentIndex % 16 == 0)
		{
			_animationColor.Play("White_Main");
		}

		else if (SegmentIndex % 8 == 0)
		{
			_animationColor.Play("White");
		}

		else if (SegmentIndex % 4 == 0)
		{
			_animationColor.Play("Red");
		}

		else if (SegmentIndex % 2 == 0)
		{
			_animationColor.Play("Blue");
		}

		else
		{
			_animationColor.Play("Yellow");
		}
    }
}
