using Godot;
using System;
using System.Collections.Generic;

public struct DataEditor
{
    public string NameProject { get; set; }
    public float BPM { get; set; }
    public int Offset { get; set; }
    public string ProjectPath { get; set; }
}

public struct DataOsu
{
    public string OsuFilePath { get; set; }
    public string OsbFilePath { get; set; }
}

public struct DataObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ObjectsTypeList ObjectsType { get; set; }
    public Dictionary<string, DataObject> Items { get; set; } //Group or storyboard object

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

        else
            return GroupType.UserCustom;
    }

    public static bool CheckSystemName(string layerName)
    {
        if (layerName == "Background" ||
            layerName == "Fail" ||
            layerName == "Pass" ||
            layerName == "Foreground" ||
            layerName == "Overlay"
            )
            return true;

        else 
            return false;
    }
}

public partial class DataObjectTreeMetadata : GodotObject
{
    public ObjectsTypeList ObjectType { get; set; }
    public DataObject DataObject { get; set; }
    public GroupType GroupType { get; set; }
}
