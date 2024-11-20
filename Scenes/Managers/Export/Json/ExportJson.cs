using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

public partial class ExportJson : Node
{
	public event Action ProjectUpdatedEvent;

	public override void _Ready()
	{

    }

	public override void _Process(double delta)
	{

	}

    public void CreateJsonFile(StoryboardData data)
	{
        string json = JsonSerializer.Serialize(data);

        using (FileStream fs = File.Create(System.Environment.CurrentDirectory + "/Project.json"))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(json);
            fs.Write(info, 0, info.Length);
        }

        ProjectUpdatedEvent?.Invoke();

    }
}
