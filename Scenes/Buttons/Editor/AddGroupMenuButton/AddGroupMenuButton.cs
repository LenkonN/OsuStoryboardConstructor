using Godot;
using System;

/// <summary>
/// Menu button in editor for create new group object.
/// </summary>
public partial class AddGroupMenuButton : TextureRect
{

	[Export] private TreeObjects _storyboardTree;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	/// <summary>
	/// Event on click button. Called create group item.
	/// </summary>
	private void OnClick()
	{
		TreeItem selectedItem = _storyboardTree.GetSelected();
		if (selectedItem == null)
			return;

        DataObjectTreeMetadata metadata = selectedItem.GetMetadata((int)TreeObjectCollumn.Text).As<DataObjectTreeMetadata>();
        if (metadata == null)
			return;

		if (metadata.DataObject.ObjectType is ObjectsTypeList.Group)
		{
			DataObject newGroup = Editor.Instance.StoryboardObjectStructureManager.CreateGroup("New group", "");
            Editor.Instance.StoryboardObjectStructureManager.AddItem(metadata.DataObject.UID, newGroup);
        }
	}
}
