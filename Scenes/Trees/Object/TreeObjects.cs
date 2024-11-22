using Godot;
using System;
using Godot.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class TreeObjects : Tree
{
	public event Action<DataObjectTreeMetadata> SelectedItemEvent;

	public ulong? LastSelectedItemUid;
	private TreeItem _mainRoot;
	

    public override void _Ready()
	{
		ExportManager.Instance.ToEditor.GroupImportEvent += AddSystemGroup;
		ExportManager.Instance.ToEditor.StartImportJsonEvent += ClearTree;
		ExportManager.Instance.ToEditor.FinishedImportJsonEvent += ReselectLastObject;
        CreateTree();
	}

	public override void _Process(double delta)
	{

	}

	private void OnSelectItem()
	{
		TreeItem selectedItem = this.GetSelected();
        DataObjectTreeMetadata metadata = selectedItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();

		if (metadata != null)
			LastSelectedItemUid = metadata.DataObject.UID;
		else if (metadata == null)
			LastSelectedItemUid = null;

        SelectedItemEvent?.Invoke(metadata);
	}

	public void ReselectLastObject()
	{
		var allItems = GetAllItems();

		foreach (var item in allItems)
		{
            DataObjectTreeMetadata metadata = item.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();

			if(metadata == null)
				continue;

			if (LastSelectedItemUid == metadata.DataObject.UID)
				item.Select((int)TreeObjectCollumn.Text);
        }
	}
    public List<TreeItem> GetAllItems()
    {
        List<TreeItem> items = new List<TreeItem>();

        CollectItems(_mainRoot, items);

        return items;
    }

    private void CollectItems(TreeItem item, List<TreeItem> items)
    {
        items.Add(item);

        TreeItem child = item.GetFirstChild();
        while (child != null)
        {
            CollectItems(child, items);
            child = child.GetNext();
        }
    }

    private void CreateTree()
	{
		Columns = 2;
		
        _mainRoot = CreateItem();
        _mainRoot.SetText((int)TreeObjectCollumn.Text, "Storyboard");
    }

	private void ClearTree()
	{
        Array<TreeItem> objects = _mainRoot.GetChildren();
		foreach (var item in objects)
			_mainRoot.RemoveChild(item);
	}

	private void AddSystemGroup(DataObject dataObject)
	{
        AddGroup(dataObject, _mainRoot);
	}

	private void AddGroup(DataObject dataObject, TreeItem parent)
	{
        Editor.Instance.StoryboardObjectList.Add(dataObject);

		TreeItem group = CreateItem(parent);

        group.SetText((int)TreeObjectCollumn.Text, dataObject.Name);
		group.SetTooltipText((int)TreeObjectCollumn.Text, dataObject.Description);

		GroupType systemLayerName = DataObjectOperation.CheckAndReturnName(dataObject.Name);

		var metadata = new DataObjectTreeMetadata()
		{
			DataObject = dataObject,
			GroupType = systemLayerName,
		};

        group.SetMetadata((int)TreeObjectCollumn.Text, metadata);

		group.SetEditable((int)TreeObjectCollumn.Text, false);
        group.SetEditable((int)TreeObjectCollumn.Icon, false);

		if (dataObject.Items != null)
		{
			foreach (var item in dataObject.Items)
			{
				if (item.Value.ObjectType == ObjectsTypeList.Group)
					AddGroup(item.Value, group);
			}
		}
    }

	private void Test()
	{
        Columns = 2;

		Texture2D texture2D = GD.Load<Texture2D>("res://icon.svg");

        TreeItem root = CreateItem();
		root.SetText(0, "Main Root");

		TreeItem group1 = CreateItem(root);
		group1.SetText(0, "UI Elements");
		group1.SetIcon(1, texture2D);

		TreeItem objectTest = CreateItem(group1);
		objectTest.SetText(0, "Object1");
		objectTest.SetCellMode(1, TreeItem.TreeCellMode.String);
		objectTest.SetEditable(1, true);

        TreeItem objectTest2 = CreateItem(group1);
        objectTest2.SetText(0, "Object2");
        objectTest2.SetCellMode(1, TreeItem.TreeCellMode.String);
        objectTest2.SetEditable(1, true);

        TreeItem objectTest3 = CreateItem(group1);
        objectTest3.SetText(0, "Object3");
        objectTest3.SetCellMode(1, TreeItem.TreeCellMode.String);
        objectTest3.SetEditable(1, true);



        TreeItem group2 = CreateItem(root);
        group2.SetText(0, "Game");

        TreeItem objectTest4 = CreateItem(group2);
        objectTest4.SetText(0, "Object4");


    }
}
