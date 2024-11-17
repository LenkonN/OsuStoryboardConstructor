using Godot;
using System;

public partial class ExportManager : Node
{
	public static ExportManager Instance { get; private set; }
	[Export] public	ExportJson Json;
    [Export] public ExportToEditor ToEditor;
    [Export] public ExportToOsb ToOsb;

    public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{

	}
}
