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
        _mainRoot.SetText((int)TreeParameterCollumn.Text, TITLE_PARAM_NONE);
        _mainRoot.SetMetadata((int)TreeParameterCollumn.Value, (int)ParamMetadataUse.Group);
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

        else if (dataObject.ObjectType is ObjectsTypeList.Sprite)
        {
            _lastSelectedTitle = ObjectsTypeList.Sprite;
            CreateSpriteParameters(dataObject);
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

    private void OnItemEdit()
    { 
        TreeItem selectedObject = _treeObjects.GetSelected();
        DataObjectTreeMetadata metadata = selectedObject.GetMetadata((int)TreeParameterCollumn.Text).As<DataObjectTreeMetadata>();

        TreeItem selectedParam = this.GetSelected();
        Dictionary<string, string> newValues = new Dictionary<string, string>();

        var allItems = GetAllItems();
        int index = 0;

        foreach (TreeItem item in allItems)
        {
            ParamMetadataUse metadataParamUse = (ParamMetadataUse)item.GetMetadata((int)TreeParameterCollumn.Value).As<int>();
            if (metadataParamUse is ParamMetadataUse.Group)
                continue;

            index++;

            if (metadata.DataObject.ObjectType is ObjectsTypeList.Group)
            {
                if (index == 1) //Name
                    newValues.Add(StaticNamesParam.Name, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 2) //Description
                    newValues.Add(StaticNamesParam.Description, item.GetText((int)TreeParameterCollumn.Value));
            }

            else if (metadata.DataObject.ObjectType is ObjectsTypeList.Sprite)
            {
                if (index == 1) //Name
                    newValues.Add(StaticNamesParam.Name, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 2) //Description
                    newValues.Add(StaticNamesParam.Description, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 3) // PositionX
                    newValues.Add(StaticNamesAttribute.Sprite.PositionX, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 4) // PositionY
                    newValues.Add(StaticNamesAttribute.Sprite.PositionY, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 5) // Rotate
                    newValues.Add(StaticNamesAttribute.Sprite.Rotate, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 6) // ScaleX
                    newValues.Add(StaticNamesAttribute.Sprite.ScaleX, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 7) // ScaleY
                    newValues.Add(StaticNamesAttribute.Sprite.ScaleY, item.GetText((int)TreeParameterCollumn.Value));

                if (index == 8) //Path
                    newValues.Add(StaticNamesAttribute.Sprite.ImagePath, item.GetText((int)TreeParameterCollumn.Value));
            }
        }

        Editor.Instance.StoryboardObjectStructureManager.UpdateItem(metadata.DataObject, newValues);
    }

    public void OnSpriteEditorInteraction(SpriteStoryboard spriteStoryboard)
    {
        TreeItem selectedObject = _treeObjects.GetSelected();
        DataObjectTreeMetadata metadata = selectedObject.GetMetadata((int)TreeParameterCollumn.Text).As<DataObjectTreeMetadata>();

        var allItems = GetAllItems();
        int index = 0;

        foreach (TreeItem item in allItems)
        {
            ParamMetadataUse metadataParamUse = (ParamMetadataUse)item.GetMetadata((int)TreeParameterCollumn.Value).As<int>();
            if (metadataParamUse is ParamMetadataUse.Group)
                continue;

            index++;

            if (metadata.DataObject.ObjectType is ObjectsTypeList.Sprite)
            {
                if (index == 3) //PositionX
                    item.SetText((int)TreeParameterCollumn.Value, spriteStoryboard.PositionStoryboard.X.ToString());

                if (index == 4) //PositionY
                    item.SetText((int)TreeParameterCollumn.Value, spriteStoryboard.PositionStoryboard.Y.ToString());

                if (index == 5) // Rotate
                    item.SetText((int)TreeParameterCollumn.Value, spriteStoryboard.RotateStoryboard.ToString());

                if (index == 6) // ScaleX
                    item.SetText((int)TreeParameterCollumn.Value, spriteStoryboard.ScaleStoryboard.X.ToString());

                if (index == 7) // ScaleY
                    item.SetText((int)TreeParameterCollumn.Value, spriteStoryboard.ScaleStoryboard.Y.ToString());

            }
        }

        OnItemEdit();
    }

    private TreeItem CreateGroup(string name, string description, TreeItem parent)
    {
        TreeItem group = CreateItem(parent);

        group.SetText((int)TreeParameterCollumn.Text, name);
        group.SetTooltipText((int)TreeParameterCollumn.Text, description);

        group.SetEditable((int)TreeParameterCollumn.Text, false);
        group.SetEditable((int)TreeParameterCollumn.Value, false);

        group.SetMetadata((int)TreeParameterCollumn.Value, (int)ParamMetadataUse.Group);

        return group;
    }
    
    private TreeItem CreateParameter(string name, string description, TreeItem parent, TreeCellMode cellMode)
    {
        TreeItem item = CreateItem(parent);

        item.SetText((int)TreeParameterCollumn.Text, name);
        item.SetTooltipText((int)TreeParameterCollumn.Text, description);


        item.SetCellMode((int)TreeParameterCollumn.Value, cellMode);

        item.SetEditable((int)TreeParameterCollumn.Text, false);
        item.SetEditable((int)TreeParameterCollumn.Value, true);

        item.SetMetadata((int)TreeParameterCollumn.Value, (int)ParamMetadataUse.Parameter);

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

            if (param.Key == StaticNamesParam.Name)
                item.SetText((int)TreeParameterCollumn.Value, dataObject.Name);

            else if (param.Key == StaticNamesParam.Description)
                item.SetText((int)TreeParameterCollumn.Value, dataObject.Description);
        }
    }

    private void CreateSpriteParameters(DataObject dataObject)
    {
        Dictionary<string, PreParamObject> paramObject = LoadJsonObject(ObjectsTypeList.Sprite);

        foreach (var param in paramObject)
        {
            if (param.Key == StaticNamesParam.Name)
            {
                TreeItem item = CreateParameter(param.Value.Name, param.Value.Description, null, param.Value.Mode);
                item.SetText((int)TreeParameterCollumn.Value, dataObject.Name);
            }

            else if (param.Key == StaticNamesParam.Description)
            {
                TreeItem item = CreateParameter(param.Value.Name, param.Value.Description, null, param.Value.Mode);
                item.SetText((int)TreeParameterCollumn.Value, dataObject.Description);
            }

            else if (param.Key == StaticNamesAttribute.Sprite.TransformGroup)
            {
                TreeItem group = CreateGroup(param.Value.Name, param.Value.Description, null);

                foreach (var paramTransform in param.Value.PreParamObjects)
                {
                    if (paramTransform.Key == StaticNamesAttribute.Sprite.PositionX)
                    {
                        TreeItem item = CreateParameter(paramTransform.Value.Name, paramTransform.Value.Description, group, paramTransform.Value.Mode);
                        item.SetText((int)TreeParameterCollumn.Value, ((DataAttributes.Sprite)dataObject.Attributes).Position[(int)Vector2Json.X].ToString());
                    }

                    else if (paramTransform.Key == StaticNamesAttribute.Sprite.PositionY)
                    {
                        TreeItem item = CreateParameter(paramTransform.Value.Name, paramTransform.Value.Description, group, paramTransform.Value.Mode);
                        item.SetText((int)TreeParameterCollumn.Value, ((DataAttributes.Sprite)dataObject.Attributes).Position[(int)Vector2Json.Y].ToString());
                    }

                    else if (paramTransform.Key == StaticNamesAttribute.Sprite.Rotate)
                    {
                        TreeItem item = CreateParameter(paramTransform.Value.Name, paramTransform.Value.Description, group, paramTransform.Value.Mode);
                        item.SetText((int)TreeParameterCollumn.Value, ((DataAttributes.Sprite)dataObject.Attributes).Rotate.ToString());
                    }

                    else if (paramTransform.Key == StaticNamesAttribute.Sprite.ScaleX)
                    {
                        TreeItem item = CreateParameter(paramTransform.Value.Name, paramTransform.Value.Description, group, paramTransform.Value.Mode);
                        item.SetText((int)TreeParameterCollumn.Value, ((DataAttributes.Sprite)dataObject.Attributes).Scale[(int)Vector2Json.X].ToString());
                    }

                    else if (paramTransform.Key == StaticNamesAttribute.Sprite.ScaleY)
                    {
                        TreeItem item = CreateParameter(paramTransform.Value.Name, paramTransform.Value.Description, group, paramTransform.Value.Mode);
                        item.SetText((int)TreeParameterCollumn.Value, ((DataAttributes.Sprite)dataObject.Attributes).Scale[(int)Vector2Json.Y].ToString());
                    }
                }
            }

            else if (param.Key == StaticNamesAttribute.Sprite.ImageGroup)
            {
                TreeItem group = CreateGroup(param.Value.Name, param.Value.Description, null);

                foreach (var paramImage in param.Value.PreParamObjects)
                {
                    if (paramImage.Key == StaticNamesAttribute.Sprite.ImagePath)
                    {
                        TreeItem item = CreateParameter(paramImage.Value.Name, paramImage.Value.Description, group, paramImage.Value.Mode);
                        item.SetText((int)TreeParameterCollumn.Value, ((DataAttributes.Sprite)dataObject.Attributes).ImagePath);
                    }
                }
            }
        }
    }
}
