using Godot;
using System;
using System.Collections.Generic;

public partial class Timeline : Control
{
	public static Timeline Instance { get; private set; }
	public event Action SelectedSegmentChangedEvent;

	public int CurrentSegmentIndexSelected { get; private set; }
	public TimelineSegment CurrentSegmentSelected { get; private set; }

	[Export] private PackedScene _segmentScene;

	[Export] private ScrollTimelineContainer _scrollContainer;
	[Export] private VFlowContainer _timelineContent;

	private float _segmentWidth;

	public int DefaultRightCountSegment = 50;

	private int _leftSegmentIndex = -300;
	private int _rightSegmentIndex = 50;

    private int _indexEdgeExpansion = 40;
	[Export] private float _timeForLoad = 0.1f;

	public bool IsLoadFinished;

	public List<TimelineSegment> SegmentList = new List<TimelineSegment>();

	private enum _sideName
	{
		Left, Right
	}

	public override void _Ready()
	{
		Instance = this;
		PopulateInitSegments();
		//_scrollContainer.ScrollHorizontalCustomStep = _segmentWidth;
	}

	public override void _Process(double delta)
	{
		if (!IsLoadFinished)
			return;

		NewSegmentByScroll();
		ScrollByWheel();
    }

	private void PopulateInitSegments()
	{
		for (int i = _leftSegmentIndex; i <= _rightSegmentIndex; i++)
		{
			AddSegment(i);
		}

		ScrollToSegment(0);
		IsLoadFinished = true;
	}

	private void AddSegment(int index)
	{
		TimelineSegment segment = _segmentScene.Instantiate<TimelineSegment>();
		_segmentWidth = segment.CustomMinimumSize.X;
		segment.SetSegment(index);
		_timelineContent.AddChild(segment);
		
		SegmentList.Add(segment);

	}

	private void NewSegmentByScroll()
	{
		if (_indexEdgeExpansion >= Math.Abs(CurrentSegmentIndexSelected - _leftSegmentIndex))
			AddAdditionalSegment(_sideName.Left);

		if (_indexEdgeExpansion >= Math.Abs(CurrentSegmentIndexSelected - _rightSegmentIndex))
			AddAdditionalSegment(_sideName.Right);

	}

	private void ScrollByWheel()
	{

		if (Input.IsActionJustPressed("ScrollLeft"))
		{
			ScrollToSegment(CurrentSegmentIndexSelected -= 1);
		}

        else if (Input.IsActionJustPressed("ScrollRight"))
        {
            ScrollToSegment(CurrentSegmentIndexSelected += 1);
        }
    }

	private async void ScrollToSegment(int index)
	{

        if (!IsLoadFinished)
            await ToSignal(GetTree().CreateTimer(_timeForLoad), "timeout");

        if (index < _leftSegmentIndex)
			return;

        int fixedIndex = Math.Abs(_leftSegmentIndex) + index;
		int targetScroll = (int)((fixedIndex * (_segmentWidth + 4)) - _scrollContainer.Size.X / 2) ;
        _scrollContainer.ScrollHorizontal = targetScroll;

        CurrentSegmentIndexSelected = index;
		CurrentSegmentSelected = _timelineContent.GetNode<TimelineSegment>(index.ToString());
        SelectedSegmentChangedEvent?.Invoke();
    }

	private void SetLastSelectedSegment(int index)
	{
		
	}

	private void AddAdditionalSegment(_sideName sideName)
	{
		//if (sideName is _sideName.Left)
		//{
		//	_leftSegmentIndex--;
		//	AddSegment(_leftSegmentIndex);
		//	ScrollToSegment(CurrentSegmentIndexSelected + 1);
		//}

		if (sideName is _sideName.Right)
		{
			_rightSegmentIndex++;
			AddSegment(_rightSegmentIndex);
		}
	}
}
