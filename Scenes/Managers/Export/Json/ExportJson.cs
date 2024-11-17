using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

public partial class ExportJson : Node
{
	public override void _Ready()
	{
        //CreateJsonFile();
    }

	public override void _Process(double delta)
	{

	}

	public void CreateJsonFile()
	{

		Dictionary<string, DataGroup> groupStoryboard = GenerateStoryboardPart();

		StoryboardData data = new StoryboardData()
		{
			Project = new StoryboardDataProject()
			{
				Editor = new DataEditor
				{
					NameProject = "Test",
					BPM = 120,
					Offset = -63,
					ProjectPath = System.Environment.CurrentDirectory

                },

				Osu = new DataOsu
				{
					OsuFilePath = "aboba/meow.osu",
					OsbFilePath = "aboba/wow.osb"
				}
			},

			Storyboard = new StoryboardDataSb()
			{
				Group = groupStoryboard
			}
		};

		string json = JsonSerializer.Serialize(data);

        using (FileStream fs = File.Create(System.Environment.CurrentDirectory + "/Project.json"))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(json);
            fs.Write(info, 0, info.Length);
        }
	}

	public Dictionary<string, DataGroup> GenerateStoryboardPart()
	{
		var group = new Dictionary<string, DataGroup>();

        for (int i = 0; i <= 4; i++)
		{
            DataGroup data = new DataGroup();
			LayerEnum name = LayerEnum.Background;

			if (i == 0)
			{
				data = CreateGroup("Background", "Back layer of the Storyboard");
				name = LayerEnum.Background;
			}

            if (i == 1)
            {
                data = CreateGroup("Fail", "Only shown if player missed");
				name = LayerEnum.Fail;
            }

            if (i == 2)
            {
                data = CreateGroup("Pass", "Only shown if player not missed");
                name = LayerEnum.Pass;
            }

            if (i == 3)
            {
                data = CreateGroup("Foreground", "Front layer of the Storyboard");
				name = LayerEnum.Foreground;
            }

            if (i == 4)
            {
                data = CreateGroup("Overlay", "Front layer of the Storyboard, overlapping also the game elements");
				name = LayerEnum.Overlay;
            }

            group.Add(name.ToString(), data);
        }

        return group;
    }

	public DataGroup CreateGroup(string nameGroup, string description)
	{
		DataGroup data = new DataGroup()
		{
			NameGroup = nameGroup,
			Description = description
		};

		return data;
    }
}
