using Godot;
using System;

/// <summary>
/// Responsible for the project environment. Stores all paths to all project files. 
/// </summary>
public partial class Environment : Node
{
	public static Environment Instance { get; private set; }

    /// <summary>
    /// Direct path to the program in AppData folder.
    /// </summary>
    public string AppdataPath { get; set; }

    /// <summary>
    /// Project folder name.
    /// </summary>
    public string FolderProjectName { get; set; }

    /// <summary>
    /// Direct path to the project folder.
    /// </summary>
    public string FolderProjectFullPath { get; set; }

    /// <summary>
    /// Project file name.
    /// </summary>
    public readonly string FileDataProjectName = "Project.json";

    /// <summary>
    /// Full path to the project file.
    /// </summary>
    public string FileProjectFullPath { get; set; }

    /// <summary>
    /// Full path to mapset folder
    /// </summary>
    public string MapsetPath { get; set; }

    /// <summary>
    /// .osu file name.
    /// </summary>
    public string FileOsuName { get; set; }

    /// <summary>
    /// .osb file name.
    /// </summary>
    public string FileOsbName { get; set; }

    /// <summary>
    /// Full path to the JsonParamObject folder.
    /// </summary>
    public string FolderJsonParamObjectPath { get; set; }



	public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{

	}

    /// <summary>
    /// Set all path values.
    /// </summary>
    /// <param name="projectName">Project name</param>
    /// <param name="mapsetPath">Project name</param>
	public void SetAllPath(string projectName, string mapsetPath)
	{
        AppdataPath = OS.GetUserDataDir();

		FolderProjectName = projectName;
        FolderProjectFullPath = $"{AppdataPath}/{FolderProjectName}";
        FileProjectFullPath = $"{FolderProjectFullPath}/{FileDataProjectName}";

        MapsetPath = mapsetPath;
        FileOsuName = null; //test value
        FileOsuName = null; //test value

        FolderJsonParamObjectPath = $"{System.Environment.CurrentDirectory}/JsonParamObject";
    }
}
