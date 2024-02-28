using System;
using Godot;
using static GlobalClass;

public partial class SensitivityBar : TextureProgressBar
{
    public override void _Process(double delta)
    {
        Value = Global.MicrophoneListener.VolumeSensitivity;
    }
}
