using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

public partial class ExportToEditor : Node
{
	public event Action<DataObject> GroupImportEvent;

	public override void _Ready()
	{
		TestStartExport();
	}

	public override void _Process(double delta)
	{

	}

	private void TestStartExport()
	{
        Godot.Timer time = new Godot.Timer()
        {
            Autostart = true,
            WaitTime = 1,
            OneShot = true,

        };

        AddChild(time);
        time.Timeout += ExportFullFile;
    }

	public void ExportFullFile()
	{
		string jsonPath = "res://Project.json";

		string jsonContent = FileAccess.Open(jsonPath, FileAccess.ModeFlags.Read).GetAsText();

		var storyboardData = JsonSerializer.Deserialize<StoryboardData>(jsonContent);

		foreach (KeyValuePair<string, DataObject> item in storyboardData.Storyboard.Group)
		{
			GroupImportEvent?.Invoke(item.Value);
		}
    }
}
