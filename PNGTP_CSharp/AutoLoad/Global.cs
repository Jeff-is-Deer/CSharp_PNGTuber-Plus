using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class GlobalClass : Node
{
	[Signal]
	public delegate void StartSpeakingEventHandler();
	[Signal]
	public delegate void StopSpeakingEventHandler();

	public static GlobalClass Global { get; set; } = null;
	public AvatarPartHeirarchyViewer AvatarPartList { set; get; } = null;
	public AvatarPartObject SelectedAvatarPart { get; set; } = null;
	public AvatarPartDetails AvatarPartDetailEdit { set; get; } = null;
	public MicrophoneListener MicrophoneListener { get; set; }
    public Main Main { get; set; } = null;
	public UserMouseCursor Mouse { set; get; } = null;
	public Node2D Failed { get; set; } = null;
	public Chain Chain { set; get; } = null;
	public PushUpdates UpdatePusherNode { set; get; } = null;
	public Color BackgroundColor { get; set; } = new Color(0.0f , 0.0f , 0.0f , 0.0f);
    public bool Filtering { get; set; } = false;
	public bool ReparentingMode { get; set; } = false;

	public string PrimaryNodeGroup { set; get; } = "NO TOUCHY >:(";
	public int AnimationTick { get; set; } = 0;
	public string CurrentAvatarFile { get; set; } = string.Empty;

	public RandomNumberGenerator RandomNum { get; } = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MicrophoneListener = new MicrophoneListener();
		MicrophoneListener.CreateMicrophone();
		Global = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		AnimationTick += 1;
		MicrophoneListener.Volume = MicrophoneListener.SpectrumAnalyzerInstance.GetMagnitudeForFrequencyRange(20.0f , 20000.0f).Length();
		if (MicrophoneListener.CurrentMicrophone != null) {
            MicrophoneListener.VolumeSensitivity = (float)Mathf.Lerp((double) MicrophoneListener.VolumeSensitivity , 0.0 , delta*2);
		}
		if ( MicrophoneListener.Volume > MicrophoneListener.VolumeLimit ) {
            MicrophoneListener.VolumeSensitivity = 1.0f;
		}
		bool previousState = MicrophoneListener.IsSpeaking;
        MicrophoneListener.IsSpeaking = MicrophoneListener.VolumeSensitivity > MicrophoneListener.SensitivityLimit;

		if (previousState != MicrophoneListener.IsSpeaking ) {
			if ( MicrophoneListener.IsSpeaking ) {
				EmitSignal(SignalName.StartSpeaking);
			}
			else {
				EmitSignal(SignalName.StopSpeaking);
			}
		}
		if (Main != null && SelectedAvatarPart != null) {
			if ( Input.IsActionJustPressed("zDown")) {
				SelectedAvatarPart.PartData.ZIndex -= 1;
				SelectedAvatarPart.SetZLayer();
				PushUpdate("Moved sprite layer.");
			}
		}
	}
	public async Task Timeout(double seconds,Node node)
	{
		await ToSignal(node.GetTree().CreateTimer(seconds) , SceneTreeTimer.SignalName.Timeout);
	}
	public async void ErrorHandler(Error error)
	{
		if(Failed == null) {
			return;
		}
		string errorMessage = string.Empty;
		switch(error) {
			case Error.FileCorrupt:
                errorMessage = "File corrupt";
                break;
			case Error.FileNotFound:
                errorMessage = "File not found";
                break;
			case Error.FileCantOpen:
                errorMessage = "File can't open";
                break;
			case Error.FileAlreadyInUse:
                errorMessage = "File in use";
                break;
			case Error.FileNoPermission:
                errorMessage = "Missing permission";
                break;
			case Error.InvalidData:
                errorMessage = "Data invalid";
                break;
			case Error.FileCantRead:
                errorMessage = "Can't read file";
                break;
			case Error.PrinterOnFire:
				errorMessage = "Unknown error";
				break;
		}
		Failed.GetNode<Label>("ErrorType").Text = errorMessage;
        Failed.Visible = true;
		await ToSignal(GetTree().CreateTimer(3) , SceneTreeTimer.SignalName.Timeout);
		Failed.Visible = false;
	}
	public void UnlinkAvatarPart()
	{
		if (SelectedAvatarPart == null || SelectedAvatarPart.PartData.PID == 0) {
			return;
		}
		Vector2 globalPos = SelectedAvatarPart.GlobalPosition;
		globalPos = new Vector2((int)globalPos.X, (int)globalPos.Y);
		SelectedAvatarPart.GetParent().RemoveChild(SelectedAvatarPart);
		Main.Origin.AddChild(SelectedAvatarPart);
		SelectedAvatarPart.Owner = Main.Origin;
		SelectedAvatarPart.PartData.PID = 0;
		SelectedAvatarPart.Position = globalPos - Main.Origin.Position;
		Global.AvatarPartList.UpdateData();
		PushUpdate("Unlinked sprite");
	}
	public void Select(Array<Area2D> areas)
	{
		if (Main.IsFileSystemOpen) {
			return;
		}
		foreach ( Area2D area in areas ) {
			if (area.IsInGroup(PrimaryNodeGroup)) {
				return;
			}
		}
		var PreviousSprite = SelectedSprite;
	}

	public void PushUpdate(string message)
	{
        if (IsInstanceIdValid(UpdatePusherNode.GetInstanceId())) {

		}
    }
}
