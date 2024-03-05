using System;
using Godot;
using Godot.Collections;
using static GlobalClass;

public partial class Avatar : Node2D
{
    public Dictionary<byte , AvatarPartObject> Parts { get; set; }
    public bool IsBlinking { get; set; }
    public bool BounceOnCostumeChange { get; set; }
    public int BounceStrength { get; set; }
    public int Gravity {  get; set; }
    public int BlinkChance { get; set; }
    public float BlinkSpeed {  get; set; }

    public override void _Ready()
    {
        
    }

    public void DeserializeSaveFile()
    {

    }

    public static int Bounce(int bounceValue) 
    {
        return bounceValue * -1;
    }
}