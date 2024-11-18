using Godot;
using System;
using Godot.Collections;
using System.Text.RegularExpressions;

public partial class TreeObjects : Tree
{
	private TreeItem _mainRoot;

    public override void _Ready()
	{
		ExportManager.Instance.ToEditor.GroupImportEvent += AddSystemGroup;
		CreateTree();
	}

	public override void _Process(double delta)
	{

	}


	private void CreateTree()
	{
		Columns = 2;
		
        _mainRoot = CreateItem();
        _mainRoot.SetText((int)TreeObjectCollumn.Text, "Storyboard");
    }

	private void AddSystemGroup(DataObject dataObject)
	{
		AddGroup(dataObject, _mainRoot);
	}

	private void AddGroup(DataObject dataObject, TreeItem parent)
	{
		TreeItem group = CreateItem(parent);

		group.SetText((int)TreeObjectCollumn.Text, dataObject.Name);
		group.SetTooltipText((int)TreeObjectCollumn.Text, dataObject.Description);

		GroupType systemLayerName = DataObjectOperation.CheckAndReturnName(dataObject.Name);

		var metadata = new DataObjectTreeMetadata()
		{
			ObjectType = ObjectsTypeList.Group,
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
				if (item.Value.ObjectsType == ObjectsTypeList.Group)
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
