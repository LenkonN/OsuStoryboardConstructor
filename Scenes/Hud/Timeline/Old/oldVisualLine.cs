using Godot;
using System;

public partial class oldVisualLine : ColorRect
{
    [Export] private ScrollContainer _scrollContainer;
    [Export] public float TotalDuration = 125f;
    [Export] private float _divisionInterval = 0.25f; //1 is 1/1, 0.5 is 1/2, 0.25 is 1/4
    [Export] private float _lineHeightBase = 20f;
	[Export] private float _bpm = 120;

    public override void _Ready()
	{
		ChangeScroll();

    }

	public override void _Process(double delta)
	{
		ChangeScroll();
	}

	public override void _Draw()
	{
		DrawTicks();
	}


	private void ChangeScroll()
	{
        float secondPerBeat = 60.0f / _bpm;
        float tickDuration = secondPerBeat * _divisionInterval;
        float pixelPerSecond = CustomMinimumSize.X / TotalDuration;
        float tickStep = tickDuration * pixelPerSecond;

		float newScrollPos = Mathf.Round(_scrollContainer.ScrollHorizontal / tickStep) * tickStep;
		_scrollContainer.ScrollHorizontal = (int)newScrollPos;


    }

    private void DrawTicks()
	{
		float secondPerBeat = 60.0f / _bpm;
		float divisionDuration = secondPerBeat * _divisionInterval;
		float pixelPerSecond = CustomMinimumSize.X / TotalDuration;
		float divisionPixelInterval = divisionDuration * pixelPerSecond;

		int colorController = 0;
        for (float x = 0; x < CustomMinimumSize.X; x += divisionPixelInterval)
		{
            Color colorSelected = Colors.White;
            float lineHeight = 0;
			

            if (colorController == 0) // blue
            {
                colorSelected = new Color("#1FAEE9");
                lineHeight = _lineHeightBase / 1.5f;
            }

            if (colorController == 1) // red
            {
                colorSelected = new Color("#FF2400");
                lineHeight = _lineHeightBase / 1.25f;
            }

            if (colorController == 2) // blue
            {
                colorSelected = new Color("#1FAEE9");
                lineHeight = _lineHeightBase / 1.5f;
            }

            if (colorController == 3) // white
            {
                colorSelected = Colors.White;
                lineHeight = _lineHeightBase / 1;
            }



            DrawLine(new Vector2(x, 5), new Vector2(x, lineHeight), colorSelected, 2);

			colorController++;
			if (colorController > 3)
				colorController = 0;
		}
	}
}
