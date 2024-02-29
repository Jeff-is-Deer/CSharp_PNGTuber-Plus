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

    public static int Bounce(int bounceValue) 
    {
        return bounceValue * -1;
    }

#nullable enable
    public Image? GetImageFromPath(string filePath)
    {
        Image result = new Image();
        Error error = result.Load(filePath);
        if (error != Error.Ok) {
            string errorName = Enum.GetName(typeof(Error) , error);
            EmitSignal(FileHandling.SignalName.ImageLoadFailed , errorName);
            return null;
        }
        return result;
    }
    public Image? GetImageFromBuffer(string base64)
    {
        Image result = new Image();
        byte[] convertedData = Marshalls.Base64ToRaw(base64);
        Error error = result.LoadPngFromBuffer(convertedData);
        if ( error != Error.Ok ) {
            string errorName = Enum.GetName(typeof(Error) , error);
            EmitSignal(FileHandling.SignalName.ImageLoadFailed , errorName);
            return null;
        }
        return result;
    }
#nullable disable

}