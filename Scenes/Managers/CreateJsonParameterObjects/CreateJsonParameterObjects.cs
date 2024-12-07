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

		else if (_needGenerate is _object.srpite)
			GenerateSpriteObject();


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
			[StaticNamesParam.Name] = new PreParamObject()
			{
				Name = "Name",
				Description = "Group name",
				PreParamObjects = null,
				Mode = TreeItem.TreeCellMode.String
			},

			[StaticNamesParam.Description] = new PreParamObject()
            {
                Name = "Description",
                Description = "Group description",
                PreParamObjects = null,
                Mode = TreeItem.TreeCellMode.String
            }
        };

		CreateJsonFile(newObject, ObjectsTypeList.Group);
    }

	private void GenerateSpriteObject()
	{
        var newObject = new Dictionary<string, PreParamObject>()
        {
            [StaticNamesParam.Name] = new PreParamObject()
            {
                Name = StaticNamesParam.Name,
                Description = "Group name",
                PreParamObjects = null,
                Mode = TreeItem.TreeCellMode.String
            },

            [StaticNamesParam.Description] = new PreParamObject()
            {
                Name = StaticNamesParam.Description,
                Description = "Group description",
                PreParamObjects = null,
                Mode = TreeItem.TreeCellMode.String
            },

            [StaticNamesAttribute.Sprite.TransformGroup] = new PreParamObject()
            {
                Name = StaticNamesAttribute.Sprite.TransformGroup,
                Description = "Transforms the position, scale and rotation of sprite",
                //Mode = null
                PreParamObjects = new Dictionary<string, PreParamObject>()
                {
                    [StaticNamesAttribute.Sprite.PositionX] = new PreParamObject()
                    {
                        Name = StaticNamesAttribute.Sprite.PositionX,
                        Description = "X coordinate of sprite position",
                        PreParamObjects = null,
                        Mode = TreeItem.TreeCellMode.String
                    },

                    [StaticNamesAttribute.Sprite.PositionY] = new PreParamObject()
                    {
                        Name = StaticNamesAttribute.Sprite.PositionY,
                        Description = "Y coordinate of sprite position",
                        PreParamObjects = null,
                        Mode = TreeItem.TreeCellMode.String
                    },

                    [StaticNamesAttribute.Sprite.Rotate] = new PreParamObject()
                    {
                        Name = StaticNamesAttribute.Sprite.Rotate,
                        Description = "Number of sprite rotations",
                        PreParamObjects = null,
                        Mode = TreeItem.TreeCellMode.String
                    },

                    [StaticNamesAttribute.Sprite.ScaleX] = new PreParamObject()
                    {
                        Name = StaticNamesAttribute.Sprite.ScaleX,
                        Description = "Scaling of object by X coordinate",
                        PreParamObjects = null,
                        Mode = TreeItem.TreeCellMode.String
                    },

                    [StaticNamesAttribute.Sprite.ScaleY] = new PreParamObject()
                    {
                        Name = StaticNamesAttribute.Sprite.ScaleY,
                        Description = "Scaling of object by Y coordinate",
                        PreParamObjects = null,
                        Mode = TreeItem.TreeCellMode.String
                    }
                }
            },

            [StaticNamesAttribute.Sprite.ImageGroup] = new PreParamObject()
            {
                Name = StaticNamesAttribute.Sprite.ImageGroup,
                Description = "Sprite image settings",
                //Mode = null
                PreParamObjects = new Dictionary<string, PreParamObject>()
                {
                    [StaticNamesAttribute.Sprite.ImagePath] = new PreParamObject()
                    {
                        Name = StaticNamesAttribute.Sprite.ImagePath,
                        Description = "Path to the image file relative to the project file",
                        PreParamObjects = null,
                        Mode = TreeItem.TreeCellMode.String
                    }
                }
            }
        };

        CreateJsonFile(newObject, ObjectsTypeList.Sprite);

    }

	
}
