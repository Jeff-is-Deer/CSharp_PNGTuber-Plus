using System;
using Godot;
using Godot.Collections;
using static GlobalClass;

public partial class Avatar : Node2D
{
    public static Dictionary<byte , AvatarPart> Parts { get; set; } 


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