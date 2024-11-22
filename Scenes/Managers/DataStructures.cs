using Godot;
using System;
using System.Collections.Generic;
using static Godot.HttpRequest;

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
    public Dictionary<string, DataObject> Items { get; set; } //Group or storyboard object

}

public struct PreParamObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<string, PreParamObject> PreParamObjects { get; set; }
    public TreeItem.TreeCellMode Mode { get; set; }
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
