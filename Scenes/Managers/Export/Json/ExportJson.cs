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
        CreateJsonFile();
    }

	public override void _Process(double delta)
	{

	}

    public void CreateJsonFile()
	{

		Dictionary<string, DataObject> groupStoryboard = GenerateStoryboardPart();

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

	public Dictionary<string, DataObject> GenerateStoryboardPart()
	{
		var group = new Dictionary<string, DataObject>();

        for (int i = 0; i <= 4; i++)
		{
            DataObject data = new DataObject();
			LayerList name = LayerList.Background;

            //----Debug

            var preSubSubGroup1 = CreateGroup("Bruh", "Pipao");
            var preSubSubGroup2 = CreateGroup("HelpMe", "Pipao2");

            var subSubGroup = new Dictionary<string, DataObject>()
            {
                [preSubSubGroup1.Name] = preSubSubGroup1,
                [preSubSubGroup2.Name] = preSubSubGroup2
            };


            var preSubGroup1 = CreateGroup("Test1", "Testing", subSubGroup);
            var preSubGroup2 = CreateGroup("Test2", "Testing");

            var subGroup = new Dictionary<string, DataObject>()
            {
                [preSubGroup1.Name] = preSubGroup1,
                [preSubGroup2.Name] = preSubGroup2
            };


            //----

            if (i == 0)
			{

				data = CreateGroup("Background", "Back layer of the Storyboard", subGroup);
				name = LayerList.Background;
			}

            if (i == 1)
            {
                data = CreateGroup("Fail", "Only shown if player missed");
				name = LayerList.Fail;
            }

            if (i == 2)
            {
                data = CreateGroup("Pass", "Only shown if player not missed");
                name = LayerList.Pass;
            }

            if (i == 3)
            {
                data = CreateGroup("Foreground", "Front layer of the Storyboard");
				name = LayerList.Foreground;
            }

            if (i == 4)
            {
                data = CreateGroup("Overlay", "Front layer of the Storyboard, overlapping also the game elements");
				name = LayerList.Overlay;
            }

            group.Add(name.ToString(), data);
        }

        return group;
    }

	public DataObject CreateGroup(string nameGroup, string description, Dictionary<string, DataObject> items = null)
	{
		DataObject data = new DataObject()
		{
			Name = nameGroup,
			Description = description,
			Items = items
		};

		return data;
    }
}
