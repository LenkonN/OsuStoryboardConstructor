using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public class DataEditor
{
    public string NameProject { get; set; }
    public float BPM { get; set; }
    public int Offset { get; set; }
    public string ProjectPath { get; set; }
}

public class DataOsu
{
    public string MappsetPath { get; set; }
    public string OsuFileName { get; set; }
    public string OsbFileName { get; set; }
}

public class DataObject
{
    public ulong UID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ObjectsTypeList ObjectType { get; set; }
    public List<KeyValuePair<string, DataObject>> Items { get; set; } //Group or storyboard object
    public object Attributes { get; set; } //Any parameters or data peculiar to unique type objects. 

}

public class DataKey
{
    public DataObject DataObject { get; set; }
    public int SegmentIndex { get; set; }
}

public class DataTimelineSegment
{
    public OsuDataTime OsuDataTime { get; private set; }
    public int SegmentIndex { get; set; }
    public DataKey DataKey { get; set; }

    public event Action TimeCountedEvent;

    public void CountTime()
    {
        OsuDataTime = new OsuDataTime()
        {
            Mil = (int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset,
            Sec = ((int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset) / 1000,
            Min = (((int)(RhythmManager.Instance.OneTickTime * SegmentIndex) + RhythmManager.Instance.Offset) / 1000) / 60
        };

        TimeCountedEvent?.Invoke();
    }
}

public struct PreParamObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<string, PreParamObject> PreParamObjects { get; set; }
    public TreeItem.TreeCellMode Mode { get; set; }
}

public class DataAttributes
{
    public class Group
    {
        public bool Collapse { get; set; }
    }

    public class Sprite
    {
        public float[] Position { get; set; } //X;Y
        public float Rotate { get; set; }
        public float[] Scale { get; set; } //X;Y
        public string ImagePath { get; set; }
    }

}

public static class DataObjectOperation
{
    public static GroupType CheckAndReturnName(string layerName)
    {
        if (layerName == "Background")
            return GroupType.Background;

        else if (layerName == "Fail")
            return GroupType.Fail;

        else if (layerName == "Pass")
            return GroupType.Pass;

        else if (layerName == "Foreground")
            return GroupType.Foreground;

        else if (layerName == "Overlay")
            return GroupType.Overlay;

        else if (layerName == "Storyboard")
            return GroupType.Storyboard;

        else
            return GroupType.UserCustom;
    }

    public static bool CheckSystemUid(ulong uid)
    {

        if (uid <= 4)
            return true;

        else 
            return false;


    }

    public static class AvaibleCheck
    {
        public static bool CheckPossibleOperationInObject(DataObject dataObject)
        {
            return CheckPossibleOperationInObjectLogic(dataObject);
        }

        public static bool CheckPossibleOperationInObject(TreeItem item)
        {
            DataObjectTreeMetadata metadata = item.GetMetadata((int)TreeParameterCollumn.Text).As<DataObjectTreeMetadata>();
            DataObject dataObject = metadata.DataObject;
            return CheckPossibleOperationInObjectLogic(dataObject);
        }

        private static bool CheckPossibleOperationInObjectLogic(DataObject dataObject)
        {
            if (dataObject.ObjectType is ObjectsTypeList.Group)
                return true;

            if (dataObject.ObjectType is ObjectsTypeList.Sprite)
                return false;

            if (dataObject.ObjectType is ObjectsTypeList.ParticleSystem)
                return false;

            return false;
        }

        public static bool CheckIsNodeObject(DataObject dataObject)
        {
            if (dataObject.ObjectType is ObjectsTypeList.Group)
                return false;

            if (dataObject.ObjectType is ObjectsTypeList.Sprite)
                return true;

            if (dataObject.ObjectType is ObjectsTypeList.ParticleSystem)
                return false;

            return false;
        }

    }
}

public partial class DataObjectTreeMetadata : GodotObject
{
    public DataObject DataObject { get; set; }
    public GroupType GroupType { get; set; }
}

public struct OsuDataTime
{
    public int Min { get; set; }
    public int Sec { get; set; }
    public int Mil { get; set; }
}
