using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

public partial class ExportToEditor : Node
{
	public event Action<DataObject> GroupImportEvent;
	public event Action StartImportJsonEvent;
	public event Action FinishedImportJsonEvent;

    private List<DataObject> _buffAllObjects = new List<DataObject>();

	public override void _Ready()
	{
        GetParent<ExportManager>().Json.ProjectUpdatedEvent += ExportFullFile;
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
		StartImportJsonEvent.Invoke();

        Editor.Instance.StoryboardObjectList.Clear();

        string jsonPath = "res://Project.json";
		string jsonContent = "";


        using (var file = FileAccess.Open(jsonPath, FileAccess.ModeFlags.Read))
        {
        	jsonContent = file.GetAsText();
        }

        StoryboardObjectStructureManager storyboardData = Editor.Instance.StoryboardObjectStructureManager;
        storyboardData.StoryboardStructureData = JsonSerializer.Deserialize<StoryboardData>(jsonContent);

		//Collect all objects in buffer
		foreach (KeyValuePair<string, DataObject> item in storyboardData.StoryboardStructureData.Storyboard.Group)
		{
            CollectAllObjects(item.Value);
        }

		//Deserialize all objects
		foreach (var item in _buffAllObjects)
		{
            DataObject objectWithDeserializeAttribute = DeserializeAllAttributes(item);
        }

        //Send all object from buff to main object list
        Editor.Instance.StoryboardObjectList = _buffAllObjects;

        //Create all tree object
        for (int i = 0; i < 5; i++)
        {
            DataObject dataObject = Editor.Instance.StoryboardObjectStructureManager.FindObject((ulong)i, storyboardData.StoryboardStructureData.Storyboard.Group);
            GroupImportEvent?.Invoke(dataObject);
        }

        FinishedImportJsonEvent?.Invoke();
    }

	private void CollectAllObjects(DataObject dataObject)
	{
        _buffAllObjects.Add(dataObject);

        if (dataObject.Items != null)
        {
            foreach (var item in dataObject.Items)
            {
                CollectAllObjects(item.Value);
            }
        }
    }

	private DataObject DeserializeAllAttributes(DataObject dataObject)
	{
		JsonElement jsonAttribute = (JsonElement)dataObject.Attributes;

		if (dataObject.ObjectType is ObjectsTypeList.Group)
		    dataObject.Attributes = jsonAttribute.Deserialize<DataAttributes.Group>();

        if (dataObject.ObjectType is ObjectsTypeList.Sprite)
            dataObject.Attributes = jsonAttribute.Deserialize<DataAttributes.Sprite>();

		return dataObject;
        
    }

}
