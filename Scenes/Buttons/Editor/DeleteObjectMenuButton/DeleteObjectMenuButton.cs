using Godot;
using System;

public partial class DeleteObjectMenuButton : TextureRect
{
	[Export] private TreeObjects _treeObjects;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	private void OnButton()
	{
		TreeItem selectedItem = _treeObjects.GetSelected();
		if (selectedItem == null)
			return;

		DataObjectTreeMetadata metadata = selectedItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
        if (metadata == null)
            return;

        Console.WriteLine(metadata.DataObject.UID);

        if (DataObjectOperation.CheckSystemUid(metadata.DataObject.UID))
			return;

        Console.WriteLine(metadata.DataObject.UID);

        TreeItem parentItem = selectedItem.GetParent();
		DataObjectTreeMetadata metadataParent = parentItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();

		Editor.Instance.StoryboardObjectStructureManager.RemoveItem(metadataParent.DataObject.UID, metadata.DataObject);
    }
}
