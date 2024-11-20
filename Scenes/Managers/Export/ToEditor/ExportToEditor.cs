using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

public partial class ExportToEditor : Node
{
	public event Action<DataObject> GroupImportEvent;
	public event Action StartImportJsonEvent;

	public override void _Ready()
	{
		TestStartExport();
        GetParent<ExportManager>().Json.ProjectUpdatedEvent += ExportFullFile;
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
		StartImportJsonEvent();

        Editor.Instance.StoryboardObjectList.Clear();

        string jsonPath = "res://Project.json";

		string jsonContent = "";


		using (var file = FileAccess.Open(jsonPath, FileAccess.ModeFlags.Read))
		{
			jsonContent = file.GetAsText();
        }

		StoryboardObjectStructureManager storyboardData = Editor.Instance.StoryboardObjectStructureManager;
        storyboardData.StoryboardStructureData = JsonSerializer.Deserialize<StoryboardData>(jsonContent);


		foreach (KeyValuePair<string, DataObject> item in storyboardData.StoryboardStructureData.Storyboard.Group)
		{
			GroupImportEvent?.Invoke(item.Value);
		}
    }
}
