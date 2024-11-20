using Godot;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using static Godot.TreeItem;
using System.Collections.Generic;

public partial class TreeParametres : Tree
{
    private static string
        TITLE_PARAM_NONE = "Object parameters",
        TITLE_PARAM_GROUP = "Group parameters",
        TITLE_PARAM_SPRITE = "Sprite parameters",
        TITLE_PARAM_SYSTEM_PARTICLES = "System particles parameters";

    [Export] private TreeObjects _treeObjects;

    private TreeItem _mainRoot;

    private ObjectsTypeList? _lastSelectedTitle;

    public override void _Ready()
	{
        CreateTree();
        _treeObjects.SelectedItemEvent += SelectedObject;
    }

	public override void _Process(double delta)
	{
        if (_lastSelectedTitle == null)
            _mainRoot.SetText((int)TreeObjectCollumn.Text, TITLE_PARAM_NONE);

        else if (_lastSelectedTitle == ObjectsTypeList.Group)
            _mainRoot.SetText((int)TreeObjectCollumn.Text, TITLE_PARAM_GROUP);

    }

    private void CreateTree()
    {
        Columns = 2;

        _mainRoot = CreateItem();
        _mainRoot.SetText((int)TreeObjectCollumn.Text, TITLE_PARAM_NONE);
        _lastSelectedTitle = null;
    }

    private void SelectedObject(DataObjectTreeMetadata metadata)
    {
        ClearTree();

        if (metadata == null)
        {
            _lastSelectedTitle = null;
            return;
        }

        DataObject dataObject = metadata.DataObject;

        if (dataObject.ObjectType is ObjectsTypeList.Group)
        {
            _lastSelectedTitle = ObjectsTypeList.Group;
            CreateGroupParameters(dataObject);
        }

        else
        {
            _lastSelectedTitle = null;
        }

    }

    private void ClearTree()
    {
        Godot.Collections.Array<TreeItem> objects = _mainRoot.GetChildren();
        foreach (var item in objects)
            _mainRoot.RemoveChild(item);
    }

    private void CreateGroup(string name, string description, TreeItem parent, DataObject dataObject)
    {
        TreeItem group = CreateItem(parent);

        group.SetText((int)TreeParameterCollumn.Text, name);
        group.SetTooltipText((int)TreeParameterCollumn.Text, description);

        group.SetEditable((int)TreeParameterCollumn.Text, false);
        group.SetEditable((int)TreeParameterCollumn.Value, false);
    }
    
    private TreeItem CreateParameter(string name, string description, TreeItem parent, TreeCellMode cellMode)
    {
        TreeItem item = CreateItem(parent);

        item.SetText((int)TreeParameterCollumn.Text, name);
        item.SetTooltipText((int)TreeParameterCollumn.Text, description);

        item.SetCellMode((int)TreeParameterCollumn.Value, cellMode);

        item.SetEditable((int)TreeParameterCollumn.Text, false);
        item.SetEditable((int)TreeParameterCollumn.Value, true);

        return item;
    }

    private Dictionary<string, PreParamObject> LoadJsonObject(ObjectsTypeList typeObject)
    {
        string jsonContent = "";

        using (var file = FileAccess.Open($"res://JsonParamObjects/{typeObject.ToString()}Object.json", FileAccess.ModeFlags.Read))
        {
            jsonContent = file.GetAsText();
        };

        Dictionary<string, PreParamObject> parameters = JsonSerializer.Deserialize<Dictionary<string, PreParamObject>>(jsonContent);

        return parameters;
    }

    private void CreateGroupParameters(DataObject dataObject)
    {
        Dictionary<string, PreParamObject> paramObject = LoadJsonObject(ObjectsTypeList.Group);

        foreach (var param in paramObject)
        {
            TreeItem item = CreateParameter(param.Value.Name, param.Value.Description, null, param.Value.Mode);

            if (param.Key == "Name")
                item.SetText((int)TreeParameterCollumn.Value, dataObject.Name);

            else if (param.Key == "Description")
                item.SetText((int)TreeParameterCollumn.Value, dataObject.Description);
        }
    }
}
