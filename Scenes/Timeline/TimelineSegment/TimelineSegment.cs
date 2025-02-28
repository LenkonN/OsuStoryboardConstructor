using Godot;
using System;
using System.Reflection;
using System.Threading.Tasks;

public partial class TimelineSegment : Control
{

	public DataTimelineSegment DataSegment = new DataTimelineSegment();

	[Export] private Label _indexLabel;
    [Export] private Label _timeLabel;
	[Export] private KeyTimeline _keyTimeline;

    [Export] private AnimationPlayer _animationSelect;
	[Export] private AnimationPlayer _animationColor;

	private bool _isAnimationLock = true;

	public override void _Ready()
	{
		TimelineCore.Instance.SelectedSegmentChangedEvent += VisualSelect;
		VisualSelect();
    }

	public override void _Process(double delta)
	{

    }

    public void SetSegment(int index)
	{
        DataSegment.TimeCountedEvent += UpdateTimeText;
        DataSegment.SegmentIndex = index;

        this.Name = index.ToString();
		TickColor();

        _indexLabel.Text = "[ " + index + " ]";

        DataSegment.CountTime();
		UpdateTimeText();
    }

	private void UpdateTimeText()
	{
        _timeLabel.Text = DataSegment.OsuDataTime.Mil + " ms";
    }

	private void VisualSelect()
	{
		if (!IsInstanceValid(this))
			return;

		if (TimelineCore.Instance.CurrentSelectSegmentNumber == DataSegment.SegmentIndex)
		{
			_isAnimationLock = false;
			_animationSelect.Play("Select");
		}

		else if (TimelineCore.Instance.CurrentSelectSegmentNumber != DataSegment.SegmentIndex && !_isAnimationLock)
		{
			_isAnimationLock = true;	
			_animationSelect.Play("NotSelect");
		}
	}

	private void TickColor()
	{

		if (DataSegment.SegmentIndex % 16 == 0)
		{
			_animationColor.Play("White_Main");
        }

		else if (DataSegment.SegmentIndex % 8 == 0)
		{
			_animationColor.Play("White");
        }

		else if (DataSegment.SegmentIndex % 4 == 0)
		{
			_animationColor.Play("Red");
        }

		else if (DataSegment.SegmentIndex % 2 == 0)
		{
			_animationColor.Play("Blue");
        }

		else
		{
			_animationColor.Play("Yellow");
        }
    }

	public void ReqCreateKey()
	{
		_keyTimeline.CreateNewDataKey();
	}

	public void ReqDeleteKey()
	{
		_keyTimeline.DeleteDataKey();
	}

	public void ReqQueueFree()
	{
        TimelineCore.Instance.SelectedSegmentChangedEvent -= VisualSelect;
        DataSegment.TimeCountedEvent -= UpdateTimeText;
        QueueFree();
	}
}
