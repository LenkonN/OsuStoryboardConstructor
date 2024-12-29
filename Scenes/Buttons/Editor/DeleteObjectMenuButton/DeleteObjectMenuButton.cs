using Godot;
using System;

/// <summary>
/// Menu button in editor. Delete selected object.
/// </summary>
public partial class DeleteObjectMenuButton : TextureRect
{
	[Export] private TreeObjects _treeObjects;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

    /// <summary>
    /// Event on click button. Calls multiple operations regarding the deletion of an object.
    /// </summary>
    private void OnButton()
	{
		TreeItem selectedItem = _treeObjects.GetSelected();
		if (selectedItem == null)
			return;

		DataObjectTreeMetadata metadata = selectedItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
        if (metadata == null)
            return;


        if (DataObjectOperation.CheckSystemUid(metadata.DataObject.UID))
			return;

		_treeObjects.ReselectAfterDelete(selectedItem);

        TreeItem parentItem = selectedItem.GetParent();
		DataObjectTreeMetadata metadataParent = parentItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();

		Editor.Instance.StoryboardObjectStructureManager.RemoveItem(metadataParent.DataObject.UID, metadata.DataObject);
    }
}
