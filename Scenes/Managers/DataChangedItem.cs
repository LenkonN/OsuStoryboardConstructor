using Godot;
using System;
using System.Collections.Generic;

public static class DataChanged
{
    public static class GroupData
    {
        public static DataObject ChangeData(DataObject dataObject, Dictionary<string, string> newValues)
        {

            if (DataObjectOperation.CheckSystemUid(dataObject.UID))
                return null;

            dataObject.Name = newValues[StaticNamesParam.Name];
            dataObject.Description = newValues[StaticNamesParam.Description];

            return dataObject;
        }
    }

    public static class SpriteData
    {
        public static DataObject ChangeData(DataObject dataObject, Dictionary<string, string> newValues)
        {
            if (DataObjectOperation.CheckSystemUid(dataObject.UID))
                return null;

            dataObject.Name = newValues[StaticNamesParam.Name];
            dataObject.Description = newValues[StaticNamesParam.Description];
            
            DataAttributes.Sprite objectAttributes = dataObject.Attributes as DataAttributes.Sprite;

            objectAttributes.Position = new float[] 
            {
                (float)Convert.ToDouble(newValues[StaticNamesAttribute.Sprite.PositionX]),
                (float)Convert.ToDouble(newValues[StaticNamesAttribute.Sprite.PositionY])
            };

            objectAttributes.Rotate = (float)Convert.ToDouble(newValues[StaticNamesAttribute.Sprite.Rotate]);

            objectAttributes.Scale = new float[]
            {
                (float)Convert.ToDouble(newValues[StaticNamesAttribute.Sprite.ScaleX]),
                (float)Convert.ToDouble(newValues[StaticNamesAttribute.Sprite.ScaleY])
            };

            objectAttributes.ImagePath = newValues[StaticNamesAttribute.Sprite.ImagePath];

            return dataObject;
        }
    }
}
