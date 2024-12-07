using Godot;
using System;
using Godot.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

public partial class TreeObjects : Tree
{
	public event Action<DataObjectTreeMetadata> SelectedItemEvent;

	public ulong? LastSelectedItemUid;
	public TreeItem LastSelectedItem;
	private TreeItem _mainRoot;
	

    public override void _Ready()
	{
		ExportManager.Instance.ToEditor.GroupImportEvent += AddSystemGroup;
		ExportManager.Instance.ToEditor.StartImportJsonEvent += ClearTree;
		ExportManager.Instance.ToEditor.FinishedImportJsonEvent += ReselectLastObject;
		
        CreateTree();

		this.DropModeFlags = (int)DropModeFlagsEnum.OnItem | (int)DropModeFlagsEnum.Inbetween;
	}

	public override void _Process(double delta)
	{			

    }

	public void ReselectAfterDelete(TreeItem targetDeleteItem)
	{
		TreeItem prevItem = targetDeleteItem.GetPrev();

		if (prevItem == null)
			prevItem = targetDeleteItem.GetParent();

		prevItem.Select((int)TreeObjectCollumn.Text);
	}

	private void OnSelectItem()
	{
		TreeItem selectedItem = this.GetSelected();
        DataObjectTreeMetadata metadata = selectedItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
		
		if (metadata != null)
		{
			LastSelectedItemUid = metadata.DataObject.UID;
			LastSelectedItem = selectedItem;
        }
		else if (metadata == null)
		{
			LastSelectedItemUid = null;
			LastSelectedItem = null;
		}

        SelectedItemEvent?.Invoke(metadata);
	}

	private void OnItemCollapsed(TreeItem item)
	{
        DataObjectTreeMetadata metadata = item.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
		
		if (metadata == null)
			return;

        DataAttributes.Group Attributes = (DataAttributes.Group)metadata.DataObject.Attributes;

        Attributes.Collapse = item.Collapsed;
    }

	public void SelectObjectByDataObject(DataObject dataObject)
	{
		var allItems = GetAllItems();

		foreach (var item in allItems)
		{
			DataObjectTreeMetadata metadata = item.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();

            if (metadata == null)
                continue;

			if (dataObject.UID == metadata.DataObject.UID)
				item.Select((int)TreeObjectCollumn.Text);
        }
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

		if(dataObject.ObjectType is ObjectsTypeList.Group)
			group.Collapsed = ((DataAttributes.Group)dataObject.Attributes).Collapse;
		
		
		if (dataObject.Items != null)
		{
			foreach (var item in dataObject.Items)
			{
				AddGroup(item.Value, group);
			}
		}
    }

    public override Variant _GetDragData(Vector2 position)
    {
		Label dragPreview = new Label();
		dragPreview.Text = LastSelectedItem.GetText((int)TreeObjectCollumn.Text);


        SetDragPreview(dragPreview);

		return LastSelectedItem;
    }

    public override bool _CanDropData(Vector2 position, Variant data)
    {
		if (data.As<TreeItem>() is TreeItem draggedItem)
		{
			TreeItem targetItem = this.GetItemAtPosition(position);

			if (targetItem != null && draggedItem != targetItem)
			{
				return true;
			}
		}
        return false;
    }

	public override void _DropData(Vector2 position, Variant data)
	{
        if (data.As<TreeItem>() is TreeItem draggedItem)
		{
			TreeItem targetItem = this.GetItemAtPosition(position);

            if (targetItem != null && targetItem != draggedItem)
			{
				TreeItem parentTargetItem = targetItem.GetParent();
				TreeItem parentDraggedItem = draggedItem.GetParent();

                if (draggedItem == _mainRoot || targetItem == _mainRoot || parentDraggedItem == _mainRoot)
					return;

				if (parentTargetItem == _mainRoot || Input.IsActionPressed("SubActive"))
					parentTargetItem = targetItem;

                if (!DataObjectOperation.AvaibleCheck.CheckPossibleOperationInObject(parentTargetItem))
                    return;

                TreeItem operationAvailabilityCheck = draggedItem;
				while (operationAvailabilityCheck != null)
				{
                    int childCount = operationAvailabilityCheck.GetChildCount();

					if (childCount != 0)
					{
                        Array<TreeItem> itemArrayCheck = operationAvailabilityCheck.GetChildren();
						foreach (TreeItem item in itemArrayCheck)
						{
							operationAvailabilityCheck = item;

							if (operationAvailabilityCheck == targetItem)
								return;
						}
					}
					else
					{
						break;
					}
				}

                DataObjectTreeMetadata draggedMeta = draggedItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
                DataObjectTreeMetadata targedMeta = targetItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
                DataObjectTreeMetadata parentTargetMeta = parentTargetItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
				DataObjectTreeMetadata parentDraggedMeta = parentDraggedItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();

                DataObject draggedObject = draggedMeta.DataObject;
                DataObject targedObject = targedMeta.DataObject;
                DataObject parentTargetObject = parentTargetMeta.DataObject;
				DataObject parentDraggedObject = parentDraggedMeta.DataObject;

				if (DataObjectOperation.CheckSystemUid(targedObject.UID) && parentTargetObject.Items.Count != 0 && !Input.IsActionPressed("SubActive"))
					return;

                int targetPosition = targetItem.GetIndex();

				Editor.Instance.StoryboardObjectStructureManager.MoveItem(draggedObject, parentDraggedObject, parentTargetObject, targetPosition);
			}
        }
	}
}
