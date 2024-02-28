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

    public void ProcessMicrophoneInput()
    {
        Volume = SpectrumAnalyzerInstance.GetMagnitudeForFrequencyRange(20.0f , 20000.0f).Length();
    }
    public void CalculateVolumeSensitivity(double delta)
    {
        VolumeSensitivity = Mathf.Lerp(VolumeSensitivity , 0.0 , delta * 2);
    }

    public async void CreateMicrophone()
    {
        Player = new AudioStreamPlayer {
            Stream = new AudioStreamMicrophone() ,
            Autoplay = true ,
            Bus = "Record"
        };
        GlobalClass.Global.AddChild(Player);
        CurrentMicrophone = Player;
        await ToSignal(GetTree().CreateTimer(MicrophoneResetTime) , SceneTreeTimer.SignalName.Timeout);
        if (CurrentMicrophone != Player) {
            return;
        }
        DeleteAllMicrophones();
        CurrentMicrophone.Dispose();
        CurrentMicrophone = null;
        await ToSignal(GetTree().CreateTimer(0.25) , SceneTreeTimer.SignalName.Timeout);
        CreateMicrophone();
    }

    private void DeleteAllMicrophones()
    {
        foreach ( Node child in GlobalClass.Global.GetChildren() ) {
            child.QueueFree();
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

