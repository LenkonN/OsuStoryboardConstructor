using Godot;
using System;

public partial class AddGroupMenuButton : TextureRect
{

	[Export] private TreeObjects _storyboardTree;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

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
			DataObject newGroup = Editor.Instance.StoryboardObjectStructureManager.CreateGroup((Editor.Instance.StoryboardObjectList.Count + 1).ToString(), "helpMe");
            Editor.Instance.StoryboardObjectStructureManager.AddItem(metadata.DataObject.Name, newGroup);


        }
	}
}
