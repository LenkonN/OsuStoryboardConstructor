using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.IO;

/// <summary>
/// Not used for program operation (only in editor). It is necessary for convenient and fast autogeneration of json files for parameter fields
/// </summary>
public partial class CreateJsonParameterObjects : Node
{
	private enum _object { none, group, srpite}
	[Export] private _object _needGenerate;

    public override void _Ready()
    {
		GenerateJson();
    }

    private void GenerateJson()
	{
		if (_needGenerate is _object.none)
			return;

		else if (_needGenerate is _object.group)
			GenerateGroupObject();

	}

	private void CreateJsonFile(Dictionary<string, PreParamObject> newObject, ObjectsTypeList fileName)
	{
        string json = JsonSerializer.Serialize(newObject);

		using (FileStream fs = File.Create($"{System.Environment.CurrentDirectory}/JsonParamObjects/{fileName}Object.json"))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(json);
            fs.Write(info, 0, info.Length);
        }
    }

	private void GenerateGroupObject()
	{

		var newObject = new Dictionary<string, PreParamObject>()
		{
			["Name"] = new PreParamObject()
			{
				Name = "Name",
				Description = "Group name",
				PreParamObjects = null,
				Mode = TreeItem.TreeCellMode.String
			},

			["Description"] = new PreParamObject()
            {
                Name = "Description",
                Description = "Group description",
                PreParamObjects = null,
                Mode = TreeItem.TreeCellMode.String
            }
        };

		CreateJsonFile(newObject, ObjectsTypeList.Group);
    }

	
}
