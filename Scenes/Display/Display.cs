using Godot;
using System;

public partial class Display : Node
{
	public static Display Instance { get; set; }

    public readonly int OriginalWindowWidth = 1280;
    public readonly int OriginalWindowHeight = 800;

	public float WindowScaleX = 1;
	public float WindowScaleY = 1;
    public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{
		CalculateScaleWindow();
	}

    private void CalculateScaleWindow()
    {
        Vector2I windowSize = DisplayServer.WindowGetSize();

        WindowScaleX = (float)windowSize.X / (float)OriginalWindowWidth;
        WindowScaleY = (float)windowSize.Y / (float)OriginalWindowHeight;
    }
}
