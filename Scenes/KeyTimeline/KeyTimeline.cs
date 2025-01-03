using Godot;
using System;

public partial class KeyTimeline : TextureRect
{
	public DataKey DataKey { get; private set; }
	public int SegmentIndex { get; private set; }
	private TimelineSegment _timelineSegment { get; set; }

	public override void _Ready()
	{
		_timelineSegment = GetNode<TimelineSegment>("../../");
	}

	public override void _Process(double delta)
	{
		VisibleChange();
	}

	public void CreateNewDataKey()
	{
		DataKey = new DataKey()
		{
			DataObject = Editor.Instance.Hud.TreeObjects.LastSelectedDataObject,
			SegmentIndex = _timelineSegment.DataSegment.SegmentIndex
		};

		_timelineSegment.DataSegment.DataKey = DataKey;
	}

	public void DeleteDataKey()
	{
		DataKey = null;
		_timelineSegment.DataSegment.DataKey = null;

    }

	private void VisibleChange()
	{
		if (DataKey == null)
			this.Visible = false;

		else if (DataKey != null)
            this.Visible = true;
    ;}
}
