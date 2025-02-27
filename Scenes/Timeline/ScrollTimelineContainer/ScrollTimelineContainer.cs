using Godot;
using System;

[Obsolete("Useless class", false)]
public partial class ScrollTimelineContainer : ScrollContainer
{
    //[Export] private CollisionShape2D _collision;

    public event Action ScrollStartedEvent;
	public event Action ScrollEndedEvent;

    private bool _isScrolling;
    private bool _isSliderMode;
    private bool _isMouseHover;

    //Need for check scroll status
    private int _scrollOldBuffer;

    public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
        //CheckScrollUser();
	}


    public void SetScrollBuffer(int scroll)
    {
        _scrollOldBuffer = scroll;
    }

    private void CheckScrollUser()
    {
        if (_scrollOldBuffer != this.ScrollHorizontal && !_isScrolling)
        {
            _isScrolling = true;
            ScrollStartedEvent?.Invoke();
        }
        
        else if (_scrollOldBuffer == this.ScrollHorizontal && _isScrolling)
        {
            _isScrolling = false;
            ScrollEndedEvent?.Invoke();
        }
    }

    //private void OnScrollCheckTimeout()
    //{
    //    if (!_isSliderMode && _timeline.IsLoadFinished)
    //        _scrollOldBuffer = this.ScrollHorizontal;
    //}
}
