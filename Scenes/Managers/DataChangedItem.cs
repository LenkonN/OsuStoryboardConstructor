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

            dataObject.Name = newValues[StaticNamesParam.GroupParam.Name];
            dataObject.Description = newValues[StaticNamesParam.GroupParam.Description];

            return dataObject;
        }
    }
}
