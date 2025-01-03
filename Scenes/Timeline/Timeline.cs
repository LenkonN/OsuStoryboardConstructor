using Godot;
using System;
using System.Collections.Generic;

public partial class Timeline : Control
{
	public static Timeline Instance { get; private set; }

	public event Action SelectedSegmentChangedEvent;
    public event Action ScrolledToSegmentEvent;

    public int CurrentSegmentIndexSelected { get; private set; }
	public TimelineSegment CurrentSegmentSelected { get; private set; }

	[Export] private PackedScene _segmentScene;

	[Export] private ScrollTimelineContainer _scrollContainer;
	[Export] private VFlowContainer _timelineContent;

	private float _segmentWidth;

	private int _leftSegmentIndex = -100;
	private int _rightSegmentIndex = 1000;

	private int _phantomLeftSegmentIndex;
    private int _phantomRightSegmentIndex;

    private int _indexEdgeExpansion = 60;
	[Export] private float _timeForLoad = 0.1f;
	[Export] public int OptimizationEdgeCount = 70;

	public bool IsLoadFinished;
	public bool ScrollLock { get; private set; }

	public List<DataTimelineSegment> DataSegmentList = new List<DataTimelineSegment>();

	public override void _Ready()
	{
		Instance = this;
		PopulateInitSegments();

		if(_indexEdgeExpansion > OptimizationEdgeCount)
			_indexEdgeExpansion = OptimizationEdgeCount--;

		ScrolledToSegmentEvent += NewSegmentByScroll;
        ExportManager.Instance.ToEditor.FinishedImportJsonEvent += RecalculateTime;

    }

	public override void _Process(double delta)
	{
        if (!IsLoadFinished)
			return;

		ScrollByWheel();
	}

	private void PopulateInitSegments()
	{
		for (int i = _leftSegmentIndex; i <= _rightSegmentIndex; i++)
		{
            DataTimelineSegment dataSegment = AddSegment(i);
			DataSegmentList.Add(dataSegment);
        }

		_phantomLeftSegmentIndex = _leftSegmentIndex;
		_phantomRightSegmentIndex = _rightSegmentIndex;
		ScrollToSegment(0);
		IsLoadFinished = true;
	}

	private DataTimelineSegment AddSegment(int index, TimelineSideName timelineSideName = TimelineSideName.Right)
	{
		TimelineSegment segment = _segmentScene.Instantiate<TimelineSegment>();
		_segmentWidth = segment.CustomMinimumSize.X;
		segment.SetSegment(index);
		_timelineContent.AddChild(segment);

		if (timelineSideName is TimelineSideName.Left)
			_timelineContent.MoveChild(segment, 0);

		return segment.DataSegment;
	}

	private void RemoveSegment(int index)
	{
		TimelineSegment segment = _timelineContent.GetNode<TimelineSegment>(index.ToString());
		segment.ReqQueueFree();
	}

	private void AddPhantomSegment(DataTimelineSegment dataSegment, TimelineSideName timelineSideName)
	{
        if (timelineSideName is TimelineSideName.Right)
            DataSegmentList.Add(dataSegment);

		if(timelineSideName is TimelineSideName.Left)
			DataSegmentList.Insert(0, dataSegment);
    }

    private void NewSegmentByScroll()
	{
		if (_indexEdgeExpansion >= Math.Abs(CurrentSegmentIndexSelected - _leftSegmentIndex))
			AddAdditionalSegment(TimelineSideName.Left);

		if (_indexEdgeExpansion >= Math.Abs(CurrentSegmentIndexSelected - _rightSegmentIndex))
			AddAdditionalSegment(TimelineSideName.Right);

	}

	private void ScrollByWheel()
	{

		if (TimelineFast.Instance.FastScrollLock)
			return;

		if (Input.IsActionJustPressed("ScrollLeft"))
		{
			ScrollToSegment(CurrentSegmentIndexSelected -= 1);
		}

		else if (Input.IsActionJustPressed("ScrollRight"))
		{
			ScrollToSegment(CurrentSegmentIndexSelected += 1);
		}
	}

	public async void ScrollToSegment(int index)
	{
        if (!IsLoadFinished)
			await ToSignal(GetTree().CreateTimer(_timeForLoad), "timeout");

		if (index < _leftSegmentIndex)
			return;

		int fixedIndex = 0;

        if (_leftSegmentIndex < 0)
			fixedIndex = Math.Abs(_leftSegmentIndex) + index;

		else if(_leftSegmentIndex >= 0)
            fixedIndex = index - Math.Abs(_leftSegmentIndex);

        int targetScroll = (int)((fixedIndex * (_segmentWidth + 4)) - _scrollContainer.Size.X / 2) ;
		_scrollContainer.ScrollHorizontal = targetScroll;
		ScrolledToSegmentEvent?.Invoke();

		CurrentSegmentIndexSelected = index;
		CurrentSegmentSelected = _timelineContent.GetNode<TimelineSegment>(index.ToString());
		SelectedSegmentChangedEvent?.Invoke();
    }

	private void RecalculateTime()
	{
		foreach (DataTimelineSegment data in DataSegmentList)
		{
			data.CountTime();
		}
	}

	private void AddAdditionalSegment(TimelineSideName sideName)
	{
		if (sideName is TimelineSideName.Left)
		{
			_leftSegmentIndex--;

			DataTimelineSegment dataSegment = AddSegment(_leftSegmentIndex, sideName);


            if (_leftSegmentIndex < _phantomLeftSegmentIndex)
            {
                _phantomLeftSegmentIndex = _leftSegmentIndex;
				AddPhantomSegment(dataSegment, sideName);
            }
        }

		if (sideName is TimelineSideName.Right)
		{
			_rightSegmentIndex++;

            DataTimelineSegment dataSegment = AddSegment(_rightSegmentIndex);

            if (_rightSegmentIndex > _phantomRightSegmentIndex)
            {
                _phantomRightSegmentIndex = _rightSegmentIndex;
                AddPhantomSegment(dataSegment, sideName);
            }

        }
    }

	public void RemoveSegmentForOptimization(TimelineSideName sideName)
	{
		if (sideName is TimelineSideName.Right)
		{
			RemoveSegment(_rightSegmentIndex);
            _rightSegmentIndex--;
        }

        if (sideName is TimelineSideName.Left)
        {
            RemoveSegment(_leftSegmentIndex);
            _leftSegmentIndex++;
        }
    }
}
