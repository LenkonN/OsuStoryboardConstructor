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
    public string OsuFilePath { get; set; }
    public string OsbFilePath { get; set; }
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
        public Vector2 Position { get; set; }
        public float Rotate { get; set; }
        public Vector2 Scale { get; set; }
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
}

public partial class DataObjectTreeMetadata : GodotObject
{
    public DataObject DataObject { get; set; }
    public GroupType GroupType { get; set; }
}
