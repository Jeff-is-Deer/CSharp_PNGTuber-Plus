using Godot;
using Godot.Collections;
using System;

public partial class GlobalClass : Node
{
	[Signal]
	public delegate void StartSpeakingEventHandler();
	[Signal]
	public delegate void StopSpeakingEventHandler();

	public static GlobalClass Global { get; set; } = null;
    public Main Main { get; set; } = null;
	public UserMouseCursor Mouse { set; get; } = null;
	public SpriteListViewer SpriteList { set; get; } = null;
	public AvatarSprite SelectedSprite { get; set; } = null;
	public AudioStreamPlayer CurrentMicrophone { set; get; } = null;
	public Node2D Failed { get; set; } = null;
	public Chain Chain { set; get; } = null;
	public PushUpdates UpdatePusherNode { set; get; } = null;
	public AudioEffectSpectrumAnalyzerInstance Spectrum { set; get; } = null;
	public SpriteViewer SpriteEdit { set; get; } = null;
	public Color BackgroundColor { get; set; } = new Color(0.0f , 0.0f , 0.0f , 0.0f);
    public bool Filtering { get; set; } = false;
	public bool IsSpeaking { get; set; } = false;
	public bool IsBlinking { get; set; } = false;
	public bool ReparentingMode { get; set; } = false;

	public string PrimaryNodeGroup { set; get; } = "NO TOUCHY >:(";

    public float Volume { get; set; } = 0.0f;
	public double VolumeLimit { get; set; } = 0.0f;
    public float VolumeSensitivity { set; get; } = 0.0f;
	public double SenseVolumeLimit { set; get; } = 0.0;
	public int AnimationTick { get; set; } = 0;
	public int MicResetTime { get; set; } = 180;

	public RandomNumberGenerator RandomNum { get; } = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global = this;
		Spectrum = AudioServer.GetBusEffectInstance(1 , 1) as AudioEffectSpectrumAnalyzerInstance;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		AnimationTick += 1;
		Volume = Spectrum.GetMagnitudeForFrequencyRange(20.0f , 20000.0f).Length();
		if (CurrentMicrophone != null) {
			VolumeSensitivity = (float)Mathf.Lerp((double)VolumeSensitivity , 0.0 , delta*2);
		}
		if (Volume > VolumeLimit) {
			VolumeSensitivity = 1.0f;
		}
		bool previousState = IsSpeaking;
		IsSpeaking = VolumeSensitivity > SenseVolumeLimit;

		if (previousState!=IsSpeaking) {
			if (IsSpeaking) {
				EmitSignal(SignalName.StartSpeaking);
			}
			else {
				EmitSignal(SignalName.StopSpeaking);
			}
		}
		if (Main != null && SelectedSprite != null) {
			if ( Input.IsActionJustPressed("zDown")) {
				SelectedSprite.SpriteData.ZLayer -= 1;
				SelectedSprite.SetZLayer();
				PushUpdate("Moved sprite layer.");
			}
		}
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

	public async void CreateMicrophone()
	{
		AudioStreamPlayer player = new AudioStreamPlayer();
		AudioStreamMicrophone microphone = new AudioStreamMicrophone();
		player.Stream = microphone;
		player.Autoplay = true;
		player.Bus = "Record";
		AddChild(player);
		CurrentMicrophone = player;
		await ToSignal(GetTree().CreateTimer(MicResetTime) , SceneTreeTimer.SignalName.Timeout);
		if (CurrentMicrophone != player) {
			return;
		}
		DeleteAllMicrophones();
		CurrentMicrophone.Dispose();
		CurrentMicrophone = null;
        await ToSignal(GetTree().CreateTimer(0.25) , SceneTreeTimer.SignalName.Timeout);
		CreateMicrophone();
    }

    public void DeleteAllMicrophones()
    {
		foreach( Node child in GetChildren() ) {
			child.Dispose();
		}
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
