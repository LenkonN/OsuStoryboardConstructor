using Godot;
using System;

public enum LayerList
{
    Background, Fail, Pass, Foreground, Overlay
}

public enum TreeObjectCollumn
{
    Text, Icon
}

public enum TreeParameterCollumn
{
    Text, Value
}

public enum GroupType
{
    Background, Fail, Pass, Foreground, Overlay, UserCustom, Storyboard
}

public enum GroupOperationName
{
    CheckAndReturnSystemLayerName, CheckSystemLayer
}

public enum ParamMetadataUse
{ 
    Group, Parameter
}

