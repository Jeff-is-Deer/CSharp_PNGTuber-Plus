using Godot;
using System;

public partial class json_AvatarPart : Node
{
    public byte ID { get; set; }
    public byte PID { get; set; }
    public byte[] Children { get; set; }
    public string FilePath { get; set; }
    public byte Type { get; set; } // Enum Type
    public string Base64ImageData { get; set; }
    public float[] ApdOffset { get; set; } // Vector2
    public float[] ApdPosition { get; set; } // Vector2
    public int AnimationSpeed { get; set; }
    public bool IsClipped { get; set; }
    public bool IgnoresBounce { get; set; }
    public byte[] VisibleOnCostumeLayer { get; set; } // What costume this sprite is visible on
    public int NumberOfFrames { get; set; }
    public Int16 RotationalLimitMaximum { get; set; }
    public Int16 RotationalLimitMinimum { get; set; }
    public int DragSpeed { get; set; }
    public int RotationalDragStrength { get; set; }
    public float StretchAmount { get; set; }
    public int XAmplitude { get; set; }
    public int YAmplitude { get; set; }
    public float XFrequency { get; set; }
    public float YFrequency { get; set; }
    public byte ShowOnBlink { get; set; }
    public byte ShowOnTalk { get; set; }
    public byte ZLayer { get; set; }
}
