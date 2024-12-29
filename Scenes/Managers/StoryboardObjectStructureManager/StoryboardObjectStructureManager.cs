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

    public override void _Ready()
    {
        if (_createProjectEditorDebug)
            ProjectBuilder.Instance.CreateProject("DevProject", "MapsetPath");
    }

    public override void _Process(double delta)
    {
        
    }

    /// <summary>
    /// Changes the object data.
    /// </summary>
    /// <param name="dataObject">Object, what needs to be changed.</param>
    /// <param name="newValues">New data.</param>
    public void UpdateItem(DataObject dataObject, Dictionary<string, string> newValues)
    {
        DataObject dataInStructre = FindObject(dataObject.UID, ProjectBuilder.Instance.StoryboardStructureData.Storyboard.Group);
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

        ProjectBuilder.Instance.RequestUpdateProjectEvent();
    }

    /// <summary>
    /// Add a new object into another object (by id).
    /// </summary>
    /// <param name="uid">Object parent id.</param>
    /// <param name="dataObject">New object.</param>
    public void AddItem(ulong uid, DataObject dataObject)
    {
        DataObject parentData = FindObject(uid, ProjectBuilder.Instance.StoryboardStructureData.Storyboard.Group);

        if (parentData == null)
            return;

        parentData?.Items.Add(new KeyValuePair<string, DataObject>(dataObject.UID.ToString(), dataObject));
        ProjectBuilder.Instance.RequestUpdateProjectEvent();
    }

    /// <summary>
    /// Delete object from another object (by id)
    /// </summary>
    /// <param name="uid">Object parent id.</param>
    /// <param name="dataObject">Object data.</param>
    public void RemoveItem(ulong uid, DataObject dataObject)
    {
        DataObject parentData = FindObject(uid, ProjectBuilder.Instance.StoryboardStructureData.Storyboard.Group);

        if (parentData == null)
            return;

        parentData?.Items.Remove(new KeyValuePair<string, DataObject>(dataObject.UID.ToString(), dataObject));
        ProjectBuilder.Instance.RequestUpdateProjectEvent();
    }

    /// <summary>
    /// Changes the parent and order of the object in project structure.
    /// </summary>
    /// <param name="draggedObject">The object to be moved.</param>
    /// <param name="parentDraggedObject">The parent of the object to be moved.</param>
    /// <param name="parentTargetObject">Parent of object to which should be moved the object to be relocated.</param>
    /// <param name="targetPosition">Object to which should be moved the object to be relocated.</param>
    public void MoveItem(DataObject draggedObject, DataObject parentDraggedObject, DataObject parentTargetObject, int targetPosition)
    {
        
        parentDraggedObject.Items.Remove(new KeyValuePair<string, DataObject>(draggedObject.UID.ToString(), draggedObject));

        if(parentTargetObject.Items.Count == 0)
            parentTargetObject.Items.Add(new KeyValuePair<string, DataObject>(draggedObject.UID.ToString(), draggedObject));
        else
            parentTargetObject.Items.Insert(targetPosition, new KeyValuePair<string, DataObject>(draggedObject.UID.ToString(), draggedObject));

        ProjectBuilder.Instance.RequestUpdateProjectEvent();
    }

    /// <summary>
    /// Searches for an object inside the group by id.
    /// </summary>
    /// <param name="targetUid">the id of the object need to find.</param>
    /// <param name="group">Group in which the object will be searched.</param>
    /// <returns>Returns the found object.</returns>
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

    /// <summary>
    /// Creating a new group.
    /// </summary>
    /// <param name="nameGroup">Group name.</param>
    /// <param name="description">Group description.</param>
    /// <param name="items">Other objects inside this group.</param>
    /// <param name="uidSpecific">Pre-specified uid.</param>
    /// <returns>New group created.</returns>
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

    /// <summary>
    /// Generates a new unique id.
    /// </summary>
    /// <returns>Unique id</returns>
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
