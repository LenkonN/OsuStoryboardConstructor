using Godot;
using System;
using static Godot.TreeItem;

public partial class TreeParametres : Tree
{
    private TreeItem _mainRoot;

    public override void _Ready()
	{
        CreateTree();
    }

	public override void _Process(double delta)
	{

	}

    private void CreateTree()
    {
        Columns = 2;

        _mainRoot = CreateItem();
        _mainRoot.SetText((int)TreeObjectCollumn.Text, "Object parameters");
    }

    private void SelectedObject(DataObject dataObject)
    {
        if (dataObject.ObjectType is ObjectsTypeList.Group)
            CreateGroupParameters();
    }

    private void CreateGroup(string name, string description, TreeItem parent)
    {
        TreeItem group = CreateItem(parent);

        group.SetText((int)TreeParameterCollumn.Text, name);
        group.SetTooltipText((int)TreeParameterCollumn.Text, description);

        group.SetEditable((int)TreeParameterCollumn.Text, false);
        group.SetEditable((int)TreeParameterCollumn.Value, false);
    }
    
    private void CreateParameter(string name, string description, TreeItem parent, TreeCellMode cellMode)
    {
        TreeItem item = CreateItem(parent);

        item.SetText((int)TreeParameterCollumn.Text, name);
        item.SetTooltipText((int)TreeParameterCollumn.Text, description);

        item.SetCellMode((int)TreeParameterCollumn.Value, cellMode);

        item.SetEditable((int)TreeParameterCollumn.Text, false);
        item.SetEditable((int)TreeParameterCollumn.Value, false);
    }

    private void CreateGroupParameters()
    {

    }
}
