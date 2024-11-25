using Godot;
using System;

public partial class UpdateMessage : Node
{
	private SystemMessageManager _messageManager;
	ulong _startUpdateTime;
	ulong _endUpdateTime;

	public override void _Ready()
	{
		_messageManager = GetParent<SystemMessageManager>();
		ExportManager.Instance.Json.ProjectStartUpdateEvent += StartUpdateProject;
        ExportManager.Instance.ToEditor.FinishedImportJsonEvent += EndUpdateProject;
    }

	public override void _Process(double delta)
	{

	}

	public void StartUpdateProject()
	{

		_startUpdateTime = Godot.Time.GetTicksMsec();

	}

	public void EndUpdateProject()
	{
        _endUpdateTime = Godot.Time.GetTicksMsec();
        float resultTime = _endUpdateTime - _startUpdateTime;
        string formatTime = (resultTime / 1000).ToString();
		_messageManager.CreateMessage($"Project Updated ({ formatTime } sec.)");
	}
}
