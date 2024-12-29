using Godot;
using System;
using System.Collections.Generic;

public partial class ProjectBuilder : Node
{
    public static ProjectBuilder Instance { get; private set; }
    [Export] public StoryboardObjectStructureManager _structureManager;
    public StoryboardData StoryboardStructureData = new StoryboardData();
    public event Action ProjectChangedEvent;

    public override void _Ready()
	{
        Instance = this;
        ProjectChangedEvent += RequestReloadProject;
    }

	public override void _Process(double delta)
	{

	}


    /// <summary>
    /// Called every time the object state changes, except for graphical elements (e.g. collapsed item). 
    /// Graphical elements do not trigger project update, i.e. they will be saved only if someone else triggers this event
    /// </summary>
    public void RequestReloadProject()
    {
        ExportManager.Instance.Json.CreateJsonFile(StoryboardStructureData);
    }

    public void CreateProject(string nameProject, string mapsetPath)
    {
        Environment.Instance.SetAllPath(nameProject, mapsetPath);

        StoryboardStructureData.Project = new StoryboardDataProject()
        {
            Editor = new DataEditor
            {
                NameProject = nameProject,
                BPM = 120, //test value
                Offset = -5, //test value
                ProjectPath = Environment.Instance.FolderProjectFullPath

            },

            Osu = new DataOsu
            {
                MappsetPath = Environment.Instance.MapsetPath,
                OsuFileName = Environment.Instance.FileOsuName,
                OsbFileName = Environment.Instance.FileOsbName
            }
        };

        StoryboardStructureData.Storyboard = new StoryboardDataSb()
        {
            Group = CreateSystemLayers()
        };

        RequestUpdateProjectEvent();
    }

    public List<KeyValuePair<string, DataObject>> CreateSystemLayers()
    {
        var group = new List<KeyValuePair<string, DataObject>>();

        for (int i = 0; i <= 4; i++)
        {
            DataObject data = new DataObject();
            LayerList name = LayerList.Background;

            if (i == 0)
            {

                data = _structureManager.CreateGroup("Background", "Back layer of the Storyboard", uidSpecific: 0);
                name = LayerList.Background;
            }

            if (i == 1)
            {
                data = _structureManager.CreateGroup("Fail", "Only shown if player missed", uidSpecific: 1);
                name = LayerList.Fail;
            }

            if (i == 2)
            {
                data = _structureManager.CreateGroup("Pass", "Only shown if player not missed", uidSpecific: 2);
                name = LayerList.Pass;
            }

            if (i == 3)
            {
                data = _structureManager.CreateGroup("Foreground", "Front layer of the Storyboard", uidSpecific: 3);
                name = LayerList.Foreground;
            }

            if (i == 4)
            {
                data = _structureManager.CreateGroup("Overlay", "Front layer of the Storyboard, overlapping also the game elements", uidSpecific: 4);
                name = LayerList.Overlay;
            }

            group.Add(new KeyValuePair<string, DataObject>(name.ToString(), data));
        }

        return group;
    }


    public void RequestUpdateProjectEvent()
	{
        ProjectChangedEvent?.Invoke();
    }



}
