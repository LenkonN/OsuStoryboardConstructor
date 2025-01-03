using Godot;
using System;

public partial class TimelineFast : Panel
{
	public static TimelineFast Instance { get; private set; }

	[Export] private PackedScene _previewPointScene;
	[Export] private Panel _sorryPanel;
	[Export] private int _safeAreaMouse = 25;
	private double _speedFastScroll = 0.1;
    private Vector2 _mousePosInMomentCheck;
	private int _marginSizeX;
	public bool FastScrollLock;
	private TimelineFastPreviewPoint _previewPoint { get; set; }
	

	public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{
		CheckForRemovePreviewPoint();
	}

	private void OnButton()
	{
		ReqFastScroll();
	}
	private int PutPoint()
	{
		_marginSizeX = DisplayServer.WindowGetSize().X - (int)this.Size.X - 20;
		int targetPoint = (int)GetGlobalMousePosition().X - _marginSizeX;
		return targetPoint;
		
    }

	private void ReqFastScroll()
	{
		int targetPoint = PutPoint();


        if (Input.IsActionPressed("SubActive"))
		{
			RemovePreviewPoint();
            CreatePreviewPoint(targetPoint);
		}

		else
		{
            _sorryPanel.Visible = true;
            FastScroll(targetPoint);
        }
    }

	private void FastScroll(int targetPoint)
	{
		if (FastScrollLock)
			return;

        FastScrollLock = true;

        int segmentTarget = ConvertTargetPointToSegment(targetPoint);
		DataTimelineSegment dataSegment = Timeline.Instance.DataSegmentList[segmentTarget];

        FastScrollProcess(dataSegment);
    }

	async private void FastScrollProcess(DataTimelineSegment dataSegment)
	{
		int targetPoint = dataSegment.SegmentIndex;

        while (Timeline.Instance.CurrentSegmentIndexSelected != targetPoint)
		{
			int stepCount = SpeedStepChange(Timeline.Instance.CurrentSegmentIndexSelected, targetPoint);

            await ToSignal(GetTree().CreateTimer(0.001), "timeout");

            for (int i = 0; i < stepCount; i++)
			{
				if (targetPoint > Timeline.Instance.CurrentSegmentIndexSelected)
				{
					Timeline.Instance.ScrollToSegment(Timeline.Instance.CurrentSegmentIndexSelected + 1);
				}

				if (targetPoint < Timeline.Instance.CurrentSegmentIndexSelected)
				{
					Timeline.Instance.ScrollToSegment(Timeline.Instance.CurrentSegmentIndexSelected - 1);
				}
			}
        }

        _sorryPanel.Visible = false;
        FastScrollLock = false;
    }

	private int SpeedStepChange(int currentPoint, int targetPoint)
	{
		int distance = Math.Abs(targetPoint - currentPoint);

        //if (distance >= 1000)
        //    return 1000;
		//
        if (distance >= 250)
           return 250;

        else if (distance >= 10)
            return 10;

        else if (distance >= 1)
			return 1;

		else
			return 0;

	}


	private void CheckForRemovePreviewPoint()
	{
        if (_previewPoint == null)
			return;

		Vector2 different = new Vector2(Math.Abs(_mousePosInMomentCheck.X - GetGlobalMousePosition().X), 
			Math.Abs(_mousePosInMomentCheck.Y - GetGlobalMousePosition().Y));

		if (different.Y > _safeAreaMouse)
			RemovePreviewPoint();

	}

	private void CreatePreviewPoint(int targetPoint)
	{
		if (_previewPoint != null)
			return;

		_mousePosInMomentCheck = GetGlobalMousePosition();

        _previewPoint = _previewPointScene.Instantiate<TimelineFastPreviewPoint>();
		_previewPoint.GlobalPosition = _mousePosInMomentCheck;
		CreateDataPreviewPoint(_previewPoint, targetPoint);
        this.AddChild(_previewPoint);
	}

	private void UpdatePreviewPoint()
	{
		if (_previewPoint == null)
			return;

		int targetPoint = PutPoint();
		CreateDataPreviewPoint(_previewPoint, targetPoint);
	}

	private void CreateDataPreviewPoint(TimelineFastPreviewPoint previewPoint, int targetPoint)
	{
		int segmentTargetPoint = ConvertTargetPointToSegment(targetPoint);
        previewPoint.ReqSetText(segmentTargetPoint);
    }

	private int ConvertTargetPointToSegment(int targetPoint)
	{
        float timelineSizeOneDivisions = this.Size.X / Timeline.Instance.DataSegmentList.Count;
        float segmentTargetPoint = targetPoint / timelineSizeOneDivisions;
        int roundSegmentTargetPoint = (int)Math.Round(segmentTargetPoint);

		return roundSegmentTargetPoint;
    }

	private void RemovePreviewPoint()
	{
		if (_previewPoint == null)
			return;

		_previewPoint.Free();
		_previewPoint = null;
	}


}
