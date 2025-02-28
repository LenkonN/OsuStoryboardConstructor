using Godot;
using System;

public partial class TimelineFast : Panel
{
	public static TimelineFast Instance { get; private set; }

    [Export] private PackedScene _previewPointScene;
	[Export] private int _safeAreaMouse = 25;
	private double _speedFastScroll = 0.1;
    private Vector2 _mousePosInMomentCheck;
	private int _marginSizeX;
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
            FastScroll(targetPoint);
        }
    }

	private void FastScroll(int targetPoint)
	{
        int segmentIndexTarget = ConvertTargetPointToSegment(targetPoint);

		TimelineCore.Instance.SwitchToSegment(segmentIndexTarget);
    }

    private void CheckForRemovePreviewPoint()
	{
        if (_previewPoint == null)
			return;

		Vector2 different = new Vector2(Math.Abs(_mousePosInMomentCheck.X - GetGlobalMousePosition().X), 
			Math.Abs(_mousePosInMomentCheck.Y - GetGlobalMousePosition().Y));

		if (different.Y > _safeAreaMouse)
			RemovePreviewPoint();

		else if (GetGlobalMousePosition().X > DisplayServer.WindowGetSize().X - _marginSizeX ||
			GetGlobalMousePosition().X < _marginSizeX)
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
        float timelineSizeOneDivisions = this.Size.X / TimelineCore.Instance.GetCountVirtualSegmentsRight();
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
