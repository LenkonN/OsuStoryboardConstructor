using Godot;
using System;

public partial class ObjectCollectionWindow : Window
{
	[Export] private WindowObjectCollectionBox _objectCollectionBox;

	private ObjectsTypeList lastSelectedItem;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		SetTextForItemSelected();
	}

	private void SetTextForItemSelected()
	{
		if (lastSelectedItem is ObjectsTypeList.Sprite)
			_objectCollectionBox.ObjectNameLabel.Text = ObjectsTypeList.Sprite.ToString();
    }

	public void SelectItem(ObjectsTypeList objectType)
	{
		lastSelectedItem = objectType;
	}

	public void OnCreateButton()
	{
		Editor.Instance.NewStoryboardObjectManager.OnButtonNewObject(lastSelectedItem);
	}

	private void OnClose()
	{
		QueueFree();
	}
}
