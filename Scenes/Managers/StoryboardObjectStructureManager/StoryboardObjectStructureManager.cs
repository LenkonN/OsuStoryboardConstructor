using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Xml.Linq;

/// <summary>
/// All logic of interaction with objects inside project
/// </summary>
public partial class StoryboardObjectStructureManager : Node
{
    [Export] private bool _createProjectEditorDebug;

    public event Action ProjectChangedEvent;

    public StoryboardData StoryboardStructureData = new StoryboardData();

    public override void _Ready()
    {
        ProjectChangedEvent += RequestReloadProject;

        if (_createProjectEditorDebug)
            CreateProject();
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

    public List<KeyValuePair<string, DataObject>> CreateSystemLayers()
    {
        var group = new List<KeyValuePair<string, DataObject>>();

        for (int i = 0; i <= 4; i++)
        {
            DataObject data = new DataObject();
            LayerList name = LayerList.Background;

            if (i == 0)
            {

                data = CreateGroup("Background", "Back layer of the Storyboard", uidSpecific: 0);
                name = LayerList.Background;
            }

            if (i == 1)
            {
                data = CreateGroup("Fail", "Only shown if player missed", uidSpecific: 1);
                name = LayerList.Fail;
            }

            if (i == 2)
            {
                data = CreateGroup("Pass", "Only shown if player not missed", uidSpecific:2);
                name = LayerList.Pass;
            }

            if (i == 3)
            {
                data = CreateGroup("Foreground", "Front layer of the Storyboard", uidSpecific: 3);
                name = LayerList.Foreground;
            }

            if (i == 4)
            {
                data = CreateGroup("Overlay", "Front layer of the Storyboard, overlapping also the game elements", uidSpecific: 4);
                name = LayerList.Overlay;
            }

            group.Add(new KeyValuePair<string, DataObject>(name.ToString(), data));
        }

        return group;
    }

    public void UpdateItem(DataObject dataObject, Dictionary<string, string> newValues)
    {
        DataObject dataInStructre = FindObject(dataObject.UID, StoryboardStructureData.Storyboard.Group);
        DataObject newData = null;

        if (dataObject.ObjectType is ObjectsTypeList.Group)
        {
            newData = DataChanged.GroupData.ChangeData(dataObject, newValues);
        }

        else if (dataObject.ObjectType is ObjectsTypeList.Sprite)
        { 
            newData = DataChanged.SpriteData.ChangeData(dataObject, newValues);
        }

        if (dataInStructre == null || newData == null)
            return;
        
        dataInStructre = newData;
        
        ProjectChangedEvent?.Invoke();
    }

    public void AddItem(ulong uid, DataObject dataObject)
    {

        DataObject parentData = FindObject(uid, StoryboardStructureData.Storyboard.Group);

        if (parentData == null)
            return;

        parentData?.Items.Add(new KeyValuePair<string, DataObject>(dataObject.UID.ToString(), dataObject));
        ProjectChangedEvent?.Invoke();
    }

    public void RemoveItem(ulong uid, DataObject dataObject)
    {
        DataObject parentData = FindObject(uid, StoryboardStructureData.Storyboard.Group);

        if (parentData == null)
            return;

        parentData?.Items.Remove(new KeyValuePair<string, DataObject>(dataObject.UID.ToString(), dataObject));
        ProjectChangedEvent?.Invoke();
    }

    public void MoveItem(DataObject draggedObject, DataObject parentDraggedObject, DataObject parentTargetObject, int targetPosition)
    {
        
        parentDraggedObject.Items.Remove(new KeyValuePair<string, DataObject>(draggedObject.UID.ToString(), draggedObject));

        if(parentTargetObject.Items.Count == 0)
            parentTargetObject.Items.Add(new KeyValuePair<string, DataObject>(draggedObject.UID.ToString(), draggedObject));
        else
            parentTargetObject.Items.Insert(targetPosition, new KeyValuePair<string, DataObject>(draggedObject.UID.ToString(), draggedObject));

        ProjectChangedEvent?.Invoke();
    }

    public DataObject FindObject(ulong targetUid, List<KeyValuePair<string, DataObject>> group)
    {
        foreach (KeyValuePair<string, DataObject> data in group)
        {
            DataObject dataObject = data.Value;

            if ( dataObject.UID == targetUid)
                return dataObject;

            if (dataObject.ObjectType is ObjectsTypeList.Group)
            {
                DataObject subData = FindObject(targetUid, dataObject.Items);
                if (subData != null)
                    return subData;
            }

            else
                continue;    
        }

        return null;
    }

    public DataObject CreateGroup(string nameGroup, string description, List<KeyValuePair<string, DataObject>> items = null, ulong? uidSpecific = null)
    {
        if (items == null)
            items = new List<KeyValuePair<string, DataObject>>();

        if (uidSpecific == null)
            uidSpecific = GenerateUID();

        DataObject data = new DataObject()
        {
            UID = (ulong)uidSpecific,
            Name = nameGroup,
            ObjectType = ObjectsTypeList.Group,
            Description = description,
            Items = items,
            Attributes = new DataAttributes.Group()
            {
                Collapse = false
            }
        };

        return data;
    }

    public ulong GenerateUID()
    {
        List<ulong> alreadyUsedUID = new List<ulong>();

        foreach (var item in Editor.Instance.StoryboardObjectList)
            alreadyUsedUID.Add(item.UID);

        ulong? uid = null;

        while (uid == null || alreadyUsedUID.Contains((ulong)uid))
        {
            uid = GD.Randi();
        }

        return (ulong)uid;
    }
}
