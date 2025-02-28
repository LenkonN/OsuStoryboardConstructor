using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class TimelineCore : Control
{
    public static TimelineCore Instance { get; private set; }

    public event Action SelectedSegmentChangedEvent;

    //The leftmost and rightmost ticks that have ever been visible (necessary for fast timeline)
    private int _virtualTickSegmentRight = 0;
    private int _virtualTickSegmentLeft = 0;

    //The leftmost and rightmost ticks that are currently created and visible
    private int _visualTickSegmentRight = 0;
    private int _visualTickSegmentLeft = 0;

    [Export] private int _generateTickRadius = 50;
    [Export] private float _segmentWidth = 30;
    public int CurrentSelectSegmentNumber { get; private set; } = 0 ;
    public TimelineSegment CurrentSegmentSelected { get; private set; } = null;

    [Export] private PackedScene _segmentScene;

    [Export] private ScrollContainer _scrollContainer;
    [Export] private VFlowContainer _timelineContent;

    private List<TimelineSegment> _visibleSegments = new List<TimelineSegment>();
    //private List<TimelineSegment> _allExistSegments = new List<TimelineSegment>();

    private bool _isSwitchSegment;

    private bool _isSwitchPartLeft;
    private bool _isSwitchPartRight;

    private bool _isLoadFinished;

    private int _debugValue;

    public override void _Ready()
    {
        Instance = this;
        Init();
    }

    public override void _Process(double delta)
    {
        if (!_isLoadFinished)
            return;

        SwitchFlagManager();
        ScrollByWheel();
        CheckGenerateRadius();
    }

    async private void Init()
    {
        
        await Task.Delay(1000);
        _isLoadFinished = true;
        SwitchFlagManager();
        SwitchToSegment(100);

    }

    public int GetCountVirtualSegments() => Mathf.Abs(_virtualTickSegmentLeft - _virtualTickSegmentRight);
    public int GetCountVirtualSegmentsRight() => Mathf.Abs(_virtualTickSegmentRight);
    public int GetCountVirtualSegmentsLeft() => Mathf.Abs(_virtualTickSegmentLeft);

    private void SwitchFlagManager()
    {
        if (!_isSwitchPartLeft && !_isSwitchPartRight)
            _isSwitchSegment = false;

        else if (_isSwitchPartLeft || _isSwitchPartRight)
            _isSwitchSegment = true;
    }

    private void SetSwitchFlag(bool flag)
    {
        _isSwitchPartLeft = flag;
        _isSwitchPartRight = flag;
    }

    private void ScrollByWheel()
    {
        if (Input.IsActionJustPressed("ScrollLeft"))
        {
            Scroll(CurrentSelectSegmentNumber - 1);
        }

        else if (Input.IsActionJustPressed("ScrollRight"))
        {
            Scroll(CurrentSelectSegmentNumber + 1);
        }
    }

    public void SwitchToSegment(int index)
    {
        SetSwitchFlag(true);
        _visualTickSegmentLeft = index;
        _visualTickSegmentRight = index;
        ClearAllSegments();
        Scroll(index);
    }

    private void Scroll(int index)
    {
        CurrentSelectSegmentNumber = index;

        int targetVisualScroll = (int)((index * (_segmentWidth + 4)) - _scrollContainer.Size.X / 2);
        _scrollContainer.ScrollHorizontal = targetVisualScroll;

        if (!_isSwitchSegment) //else... check GenerateSegmentsForSide func
        {
            foreach (TimelineSegment segment in _visibleSegments)
            {
                if (CurrentSelectSegmentNumber == segment.DataSegment.SegmentIndex)
                {
                    CurrentSegmentSelected = segment;
                }
            }
        }

        SelectedSegmentChangedEvent?.Invoke();
    }

    private void CheckGenerateRadius()
    {
        int bothSideRadius = _generateTickRadius / 2;

        Console.WriteLine("vtsr: " + _visualTickSegmentRight);

        //Create for right
        if (Mathf.Abs(CurrentSelectSegmentNumber - _visualTickSegmentRight) < bothSideRadius)
        {
            int needGenerateToRight = CurrentSelectSegmentNumber - _visualTickSegmentRight + bothSideRadius;

            //fix, for first line
            if (_isSwitchSegment)
            {
                needGenerateToRight++;
            }

            GenerateSegmentsForSide(TimelineSideName.Right, Math.Abs(needGenerateToRight));
            _visualTickSegmentRight = CurrentSelectSegmentNumber + bothSideRadius;

            if (_virtualTickSegmentRight < _visualTickSegmentRight)
                _virtualTickSegmentRight = _visualTickSegmentRight;
        }

        //Delete for right
        if (Mathf.Abs(CurrentSelectSegmentNumber - _visualTickSegmentRight) > bothSideRadius)
        {
            int needDeleteToRight = _visualTickSegmentRight - (bothSideRadius + CurrentSelectSegmentNumber);
            DeleteSegmentsForSide(TimelineSideName.Right, needDeleteToRight);
            _visualTickSegmentRight = CurrentSelectSegmentNumber + bothSideRadius;
        }

        //Create for left
        if (Mathf.Abs(CurrentSelectSegmentNumber - _visualTickSegmentLeft) < bothSideRadius)
        {
            int needGenerateToLeft = _visualTickSegmentLeft - CurrentSelectSegmentNumber + bothSideRadius;
            GenerateSegmentsForSide(TimelineSideName.Left, Math.Abs(needGenerateToLeft));
            _visualTickSegmentLeft = CurrentSelectSegmentNumber - bothSideRadius;

            if (_virtualTickSegmentLeft > _visualTickSegmentLeft)
                _virtualTickSegmentLeft = _visualTickSegmentLeft;
        }

        //Delete for left
        if (Mathf.Abs(CurrentSelectSegmentNumber - _visualTickSegmentLeft) > bothSideRadius)
        {
            int needDeleteToLeft = CurrentSelectSegmentNumber - (_visualTickSegmentLeft + bothSideRadius);
            DeleteSegmentsForSide(TimelineSideName.Left, needDeleteToLeft);
            _visualTickSegmentLeft = CurrentSelectSegmentNumber - bothSideRadius;
        }

    }

    private void ClearAllSegments()
    {
        for (int i = _timelineContent.GetChildCount() - 1; i >= 0; i--)
        {
            Node node = _timelineContent.GetChild(i);
            _debugValue = _timelineContent.GetChildCount();
            node.Free();
        }

        _visibleSegments.Clear();
    }

    private void GenerateSegmentsForSide(TimelineSideName side, int currentInsufficientCount)
    {
        int needGenerate = currentInsufficientCount;
        int newIndex = 0;

        if (side is TimelineSideName.Right)
            newIndex = _visualTickSegmentRight;
        
        else if (side is TimelineSideName.Left)
            newIndex = _visualTickSegmentLeft;

        //fix, for only one tick on current selected time
        if (side == TimelineSideName.Left && _isSwitchSegment)
        {
            newIndex--;
        }

        while (needGenerate > 0)
        {
            if (side == TimelineSideName.Right)
            {
                //fix for first line
                if (_isSwitchSegment)
                {
                    TimelineSegment segment = CreateSegment(newIndex, side);
                    CurrentSegmentSelected = segment;
                }
                else if (!_isSwitchSegment)
                {
                    CreateSegment(newIndex + 1, side);
                }
                
                newIndex++;
            }

            else if (side == TimelineSideName.Left)
            {
                if (_isSwitchSegment)
                {
                    CreateSegment(newIndex, side);
                }
                else if (!_isSwitchSegment)
                {
                    CreateSegment(newIndex - 1, side);
                }

                newIndex--;
            }

            needGenerate--;
        }

        if (side is TimelineSideName.Left)
            _isSwitchPartLeft = false;

        if (side is TimelineSideName.Right)
            _isSwitchPartRight = false;
    }

    private void DeleteSegmentsForSide(TimelineSideName side, int currentSuperfluousCount)
    {
        int needDelete = currentSuperfluousCount;

        while (needDelete > 0)
        {
            int deleteIndex = 0;

            if (side is TimelineSideName.Right)
            {
                deleteIndex = _visualTickSegmentRight;
            }

            else if (side is TimelineSideName.Left)
            {
                deleteIndex = _visualTickSegmentLeft;
            }

            DeleteSegment(deleteIndex, side);

            needDelete--;
        }
    }


    private TimelineSegment CreateSegment(int index, TimelineSideName side = TimelineSideName.Right)
    {
        TimelineSegment segment = _segmentScene.Instantiate<TimelineSegment>();

        segment.SetSegment(index);
        _timelineContent.AddChild(segment);

        if (side is TimelineSideName.Left)
            _timelineContent.MoveChild(segment, 0);

        _visibleSegments.Add(segment);

        return segment;
    }

    private void DeleteSegment(int index, TimelineSideName side = TimelineSideName.Right)
    {
        for (int i = 0; i < _visibleSegments.Count; i++)
        {
            TimelineSegment segment = _visibleSegments[i];
            if (segment.DataSegment.SegmentIndex == index)
            {
                _visibleSegments.Remove(segment);
                segment.QueueFree();
                break;
            }
        }
    }
}
