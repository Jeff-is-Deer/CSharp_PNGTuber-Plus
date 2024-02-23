using Godot;
using System;
using static GlobalClass;

public partial class VolumeSlider : HSlider
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        Global.VolumeLimit = MaxValue - Value;
        Saving.Settings.Volume = Value;
    }
}
