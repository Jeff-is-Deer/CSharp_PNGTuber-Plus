using System;
using Godot;

public partial class MicrophoneListener : Node
{
    [Signal]
    public delegate void StartSpeakingEventHandler();
    [Signal]
    public delegate void StopSpeakingEventHandler();

    public AudioStreamPlayer CurrentMicrophone { get; set; }
    public AudioStreamPlayer Player { get; set; }
    public AudioStreamMicrophone Microphone { get; set; }
    public AudioEffectSpectrumAnalyzerInstance SpectrumAnalyzerInstance { get; set; }

    public int MicrophoneResetTime { get; set; } = 180;
    public bool IsSpeaking { get; set; } = false;
    public double Volume { get; set; }
    public double VolumeLimit { get; set; }
    public double SensitivityLimit { get; set; }
    public double VolumeSensitivity { get; set; }
    
    public MicrophoneListener()
    {
        StartSpeaking += event_StartSpeaking;
        StopSpeaking += event_StopSpeaking;
        SpectrumAnalyzerInstance = AudioServer.GetBusEffectInstance(1 , 1) as AudioEffectSpectrumAnalyzerInstance;

    }

    public async void CreateMicrophone()
    {
        Player = new AudioStreamPlayer();
        Microphone = new AudioStreamMicrophone();
        Player.Stream = Microphone;
        Player.Autoplay = true;
        Player.Bus = "Record";
        GlobalClass.Global.AddChild(Player);
        CurrentMicrophone = Player;
        await ToSignal(GetTree().CreateTimer(MicrophoneResetTime) , SceneTreeTimer.SignalName.Timeout);
        if (CurrentMicrophone != Player) {
            return;
        }
        DeleteAllMicrophones();
        CurrentMicrophone.Dispose();
        CurrentMicrophone = null;
        CreateMicrophone();
    }

    private void DeleteAllMicrophones()
    {
        foreach ( Node child in GlobalClass.Global.GetChildren() ) {
            child.Dispose();
        }
    }

    private void event_StartSpeaking()
    {
        return;
    }
    private void event_StopSpeaking()
    {
        return;
    }
}

