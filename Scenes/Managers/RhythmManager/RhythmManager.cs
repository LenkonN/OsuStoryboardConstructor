using Godot;
using System;

public partial class RhythmManager : Node
{
	public static RhythmManager Instance { get; private set; }

	public int CurrentSelectedTime;
	[Export] public int MaxSecLengthSong = 120; //test value
	public int Division = 8; // 1/8 tick

    public float BPM;
	public float MsPerBeat;
	public float OneTickTime;
	public int Offset;
	



    public override void _Ready()
	{
		ExportManager.Instance.ToEditor.FinishedImportJsonEvent += LoadRhythmData;
		Instance = this;
	}

	public void LoadRhythmData()
	{
		StoryboardDataProject projectData = ProjectBuilder.Instance.StoryboardStructureData.Project;

		BPM = projectData.Editor.BPM;
		Offset = projectData.Editor.Offset;

		CountRhythm();
	}

	private void CountRhythm()
	{
        MsPerBeat = (60 / BPM) * 1000;
        OneTickTime = MsPerBeat / Division;
    }

	public override void _Process(double delta)
	{

	}
}
