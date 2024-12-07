using Godot;
using System;

public static class StaticNamesParam
{
     public static readonly string
          Name = "Name",
          Description = "Description";
    
}

public static class StaticNamesAttribute
{
    public static class Group
    {
        public static readonly string
            Collapse = "Collapse";
    }

    public static class Sprite
    {
        public static readonly string
            TransformGroup = "Transform",
            ImageGroup = "Image",

            PositionX = "PositionX",
            PositionY = "PositionY",
            Rotate = "Rotate",
            ScaleX = "ScaleX",
            ScaleY = "ScaleY",
            ImagePath = "Path";
    }
}
