using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

public partial class StoryboardNodeObjectManager : Node
{
	public event Action<ObjectNodeStoryboard> SelectNodeObjectEvent;
	public ObjectNodeStoryboard LastSelectedObjectNode;

	[Export] private Marker2D _spawnPos;
	[Export] private PackedScene _spriteObjectScene;

	//buffer for delete unused nodes
    private List<ObjectNodeStoryboard> _existOldNodeList = new List<ObjectNodeStoryboard>();

    public override void _Ready()
	{
        ExportManager.Instance.ToEditor.FinishedImportJsonEvent += UpdateAllDataObjects;
        ExportManager.Instance.ToEditor.FinishedImportJsonEvent += StartProjectLoadAllNodes;

    }

	public override void _Process(double delta)
	{

	}

	public void OnSelectObject(ObjectNodeStoryboard objectNode)
	{
		SelectNodeObjectEvent?.Invoke(objectNode);
		LastSelectedObjectNode = objectNode;
        Editor.Instance.Hud.TreeObjects.SelectObjectByDataObject(objectNode.DataObject);

    }

	public void UpdateAllDataObjects()
	{
        if (Editor.Instance.IsFirstLoad)
            return;

        _existOldNodeList.Clear();

        foreach (DataObject dataObject in Editor.Instance.StoryboardObjectList)
		{
			UpdateDataObject(dataObject);
		}

		DeleteUnusedNodeObject();
	}

	public ObjectNodeStoryboard FindNodeObjectByMetadata(DataObjectTreeMetadata metadata)
	{
		foreach (ObjectNodeStoryboard objectNode in Editor.Instance.StoryboardNodeList)
		{
			if(objectNode.DataObject == metadata.DataObject)
				return objectNode;
		}

		return null;
	}

	private void DeleteUnusedNodeObject()
	{
		for (int i = Editor.Instance.StoryboardNodeList.Count - 1; i >= 0; i--)
		{
			if (!_existOldNodeList.Contains(Editor.Instance.StoryboardNodeList[i]))
			{
				ObjectNodeStoryboard nodeToDeleteReq = Editor.Instance.StoryboardNodeList[i];
				nodeToDeleteReq.QueueFreeObject();
                Editor.Instance.StoryboardNodeList.Remove(nodeToDeleteReq);
            }
		}
    }

	private void UpdateDataObject(DataObject dataLoadObject)
	{
        foreach (ObjectNodeStoryboard objectStoryboard in Editor.Instance.StoryboardNodeList)
        {
			if (objectStoryboard.DataObject.UID == dataLoadObject.UID)
			{
				objectStoryboard.LoadDataObject(dataLoadObject);
                _existOldNodeList.Add(objectStoryboard);
            }
        }
    }

	public void OnButtonNewObject(ObjectsTypeList objectType)
	{
		if (!DataObjectOperation.AvaibleCheck.CheckPossibleOperationInObject(Editor.Instance.Hud.TreeObjects.LastSelectedItem))
			return;

		if (objectType is ObjectsTypeList.Sprite)
			CreateNewSprite(false);
	}

	public void StartProjectLoadAllNodes()
	{

        if (!Editor.Instance.IsFirstLoad)
			return;

        foreach (DataObject dataLoadObject in Editor.Instance.StoryboardObjectList)
		{
			if (dataLoadObject.ObjectType is ObjectsTypeList.Sprite)
			{
                CreateNewSprite(true, dataLoadObject);
			}
		}

        Editor.Instance.LockFlagFirstUpdate();
    }

	public void CreateNewSprite(bool isLoadData, DataObject dataLoadObject = null)
	{

        if (!isLoadData)
		{
            SpriteStoryboard spriteObject = _spriteObjectScene.Instantiate<SpriteStoryboard>();
            Editor.Instance.StoryboardCanvasLayer.AddChild(spriteObject);
            DataObject dataObject = spriteObject.CreateDataObject(_spawnPos.GlobalPosition, _spawnPos.Rotation, _spawnPos.Scale);
            Editor.Instance.StoryboardNodeList.Add(spriteObject);
            Editor.Instance.StoryboardObjectStructureManager.AddItem((ulong)Editor.Instance.Hud.TreeObjects.LastSelectedItemUid, dataObject);
        }

		else if (isLoadData)
		{
			if (Editor.Instance.IsFirstLoad)
			{
				SpriteStoryboard spriteObject = _spriteObjectScene.Instantiate<SpriteStoryboard>();
				Editor.Instance.StoryboardCanvasLayer.AddChild(spriteObject);
				spriteObject.LoadDataObject(dataLoadObject);
                Editor.Instance.StoryboardNodeList.Add(spriteObject);
			}
        }

	}
}
