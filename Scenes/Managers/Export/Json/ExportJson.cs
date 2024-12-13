using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

public partial class ExportJson : Node
{
	public event Action ProjectUpdatedEvent;
    public event Action ProjectStartUpdateEvent;

	public override void _Ready()
	{

    }

	public override void _Process(double delta)
	{

	}

    public void CreateJsonFile(StoryboardData data)
	{
        ProjectStartUpdateEvent?.Invoke();
        JsonProccess(data);
        ProjectUpdatedEvent?.Invoke();
    }

    public void SaveJsonFileWithoutEvents(StoryboardData data)
    {
        JsonProccess(data);
    }

    private void JsonProccess(StoryboardData data)
    {
        string json = JsonSerializer.Serialize(data);

        Console.WriteLine("---------------");
        Console.WriteLine("Export: " + json);

        using (FileStream fs = File.Create("Project.json"))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(json);
            fs.Write(info, 0, info.Length);
        };
    }
}
