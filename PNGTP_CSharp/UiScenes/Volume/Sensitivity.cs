using System;
using Godot;
using static GlobalClass;

public partial class Sensitivity : TextureProgressBar
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        Value = Global.VolumeSensitivity;
    }
}
