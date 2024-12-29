using Godot;
using System;

public enum LayerList
{
    Background, Fail, Pass, Foreground, Overlay
}

public enum UserControl
{
    None, Move, Rotate, Scale
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

public enum Vector2Json
{
    X, Y
}

