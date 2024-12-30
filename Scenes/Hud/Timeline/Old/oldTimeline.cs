using Godot;
using System;

public partial class oldTimeline : Control
{
	[Export] private ScrollContainer _scrollContainer;
	[Export] private oldVisualLine _visualLine;
	[Export] private Label _timeLabel;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		UpdateTime();
	}

	private void UpdateTime()
	{
		float scrollRatio = _scrollContainer.ScrollHorizontal / (_visualLine.CustomMinimumSize.X - _scrollContainer.Size.X);
		float currentTimeSec = scrollRatio * _visualLine.TotalDuration;
		float currentTime = currentTimeSec * 1000;
		float currentTimeRound = (float)Math.Round(currentTime);

        string msec = ((int)(currentTimeRound % 1000)).ToString("D3");
        string sec = ((int)(currentTimeRound / 1000) % 60).ToString("D2");
        string min = ((int)(currentTimeRound / 60000)).ToString("D2");

		_timeLabel.Text = $"{min}:{sec}:{msec} ({currentTimeRound})";

    }
}
