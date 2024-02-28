using Godot;
using System;
using static GlobalClass;

public partial class VolumeSlider : HSlider
{
    public override void _Process(double delta)
    {
        Global.MicrophoneListener.VolumeLimit = MaxValue - Value;
    }
}
