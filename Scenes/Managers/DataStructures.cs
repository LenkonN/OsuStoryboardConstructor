using Godot;
using System;

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

public struct DataGroup
{
    public string NameGroup { get; set; }
    public string Description { get; set; }

}

public struct DataObjectTreeMetadata
{
    public ObjectsTypeList ObjectType { get; set; }
    public DataGroup Group { get; set; }
    public GroupType GroupType { get; set; }
}
