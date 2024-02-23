using Godot;
using System;
using static GlobalClass;

public partial class VolumeBar : TextureProgressBar
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        Value = Global.Volume;
    }
}

