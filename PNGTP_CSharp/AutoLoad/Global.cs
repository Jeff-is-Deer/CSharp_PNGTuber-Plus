using Godot;
using Godot.Collections;
using System;
using System.Threading;

public partial class GlobalClass : Node
{
	[Signal]
	public delegate void StartSpeakingEventHandler();
	[Signal]
	public delegate void StopSpeakingEventHandler();

	public static GlobalClass Global { get; set; } = null;
	public Main Main { get; set; } = null;
	public UserMouseCursor Mouse { set; get; } = null;
	public SpriteObject HeldSprite { get; set; } = null;
	public AudioStreamPlayer CurrentMicrophone { set; get; } = null;
	public Node2D Failed { get; set; } = null;
	public AudioEffectSpectrumAnalyzerInstance Spectrum { set; get; } = null;
    public bool Filtering { get; set; } = false;
	public bool IsSpeaking { get; set; } = false;
	public bool IsBlinking { get; set; } = false;

	public string PrimaryNodeGroup { set; get; } = "NO TOUCHY >:(";

    public float Volume { get; set; } = 0.0f;
	public float VolumeLimit { get; set; } = 0.0f;
    public float VolumeSensitivity { set; get; } = 0.0f;
	public float SenseVolumeLimit { set; get; } = 0.0f;
	public int AnimationTick { get; set; } = 0;
	public int MicResetTime { get; set; } = 180;

	RandomNumberGenerator RandomNum { get; } = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global = this;
		Spectrum = AudioServer.GetBusEffectInstance(1 , 1) as AudioEffectSpectrumAnalyzerInstance;

		if (!Saving.Settings)
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
		if (Main != null && HeldSprite != null) {
			if ( Input.IsActionJustPressed("zDown")) {
				HeldSprite.ZLayer -= 1;
				HeldSprite.SetZLayer();
				PushUpdate("Moved sprite layer.");
			}
		}
	}

	public async void ErrorHandler(Error error)
	{
		if(Failed == null) {
			return;
		}
		Failed.GetNode<Label>("ErrorType").Text = string.Empty;
		switch(error) {
			case Error.FileCorrupt:
				Failed.GetNode<Label>("ErrorType").Text = "File corrupt";
                break;
			case Error.FileNotFound:
				Failed.GetNode<Label>("ErrorType").Text = "File not found";
                break;
			case Error.FileCantOpen:
				Failed.GetNode<Label>("ErrorType").Text = "File can't open";
                break;
			case Error.FileAlreadyInUse:
				Failed.GetNode<Label>("ErrorType").Text = "File in use";
                break;
			case Error.FileNoPermission:
				Failed.GetNode<Label>("ErrorType").Text = "Missing permission";
                break;
			case Error.InvalidData:
				Failed.GetNode<Label>("ErrorType").Text = "Data invalid";
                break;
			case Error.FileCantRead:
				Failed.GetNode<Label>("ErrorType").Text = "Can't read file";
                break;
		}
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
		var PreviousSprite = HeldSprite;
	}
}
