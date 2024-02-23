using System;
using Godot;
using static GlobalClass;
public partial class SensitivitySlider : HSlider
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        Global.SenseVolumeLimit = MaxValue - Value;
        Saving.Settings.Sensitivity = Value;
    }
}
