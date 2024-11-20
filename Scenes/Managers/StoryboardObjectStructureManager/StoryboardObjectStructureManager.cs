using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class StoryboardObjectStructureManager : Node
{
    public event Action ProjectChangedEvent;

    public StoryboardData StoryboardStructureData = new StoryboardData();

    public override void _Ready()
    {
        ProjectChangedEvent += RequestReloadProject;
    }

    public override void _Process(double delta)
    {
        
    }


    public void RequestReloadProject()
    {
        ExportManager.Instance.Json.CreateJsonFile(StoryboardStructureData);
    }

    public void CreateProject()
    {
        StoryboardStructureData.Project = new StoryboardDataProject()
        {
            Editor = new DataEditor
            {
                NameProject = "Test", //test value
                BPM = 120, //test value
                Offset = -63, //test value
                ProjectPath = System.Environment.CurrentDirectory

            },

            Osu = new DataOsu
            {
                OsuFilePath = "aboba/meow.osu", //test value
                OsbFilePath = "aboba/wow.osb" //test value
            }
        };

        StoryboardStructureData.Storyboard = new StoryboardDataSb()
        {
            Group = CreateSystemLayers()
        };

        ProjectChangedEvent?.Invoke();
    }

    public Dictionary<string, DataObject> CreateSystemLayers()
    {
        var group = new Dictionary<string, DataObject>();

        for (int i = 0; i <= 4; i++)
        {
            DataObject data = new DataObject();
            LayerList name = LayerList.Background;

            if (i == 0)
            {

                data = CreateGroup("Background", "Back layer of the Storyboard");
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

    public void AddItem(string parentName, DataObject dataObject)
    {
        DataObject? parentData = FindObject(parentName, StoryboardStructureData.Storyboard.Group);

        if (parentData == null)
            return;

        parentData?.Items.Add(dataObject.Name, dataObject);
        ProjectChangedEvent?.Invoke();
    }

    private DataObject? FindObject(string target, Dictionary<string, DataObject> group)
    {
        foreach (KeyValuePair<string, DataObject> data in group)
        {
            DataObject dataObject = data.Value;

            if ( dataObject.Name == target )
                return dataObject;

            if (dataObject.ObjectType is ObjectsTypeList.Group)
            {
                DataObject? subData = FindObject(target, dataObject.Items);
                if (subData != null)
                    return subData;
            }

            else
                return null;        
        }

        return null;
    }

    public DataObject CreateGroup(string nameGroup, string description, Dictionary<string, DataObject> items = null)
    {
        if (items == null)
            items = new Dictionary<string, DataObject>();

        DataObject data = new DataObject()
        {
            Name = nameGroup,
            ObjectType = ObjectsTypeList.Group,
            Description = description,
            Items = items
        };

        return data;
    }
}
