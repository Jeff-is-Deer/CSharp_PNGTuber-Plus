using System;

public class SpriteDataClass
{
    public byte ID { get; set; }
    public byte PID { get; set; }
    public string FilePath { get; set; }
    public byte Type { get; set; } // Enum Type
    public string Base64ImageData { get; set; }
    public float[] Offset { get; set; } // Vector2
    public float[] Position { get; set; } // Vector2
    public int AnimationSpeed { get; set; }
    public bool IsClipped { get; set; }
    public bool IgnoresBounce { get; set; }
    public bool[] VisibleOnCostumeLayer { get; set; } // What costume this sprite is visible on
    public int NumberOfFrames { get; set; }
    public Int16 RotationalLimitMaximum { get; set; }
    public Int16 RotationalLimitMinimum { get; set; }
    public int Drag { get; set; }
    public int RotationalDragStrength {  get; set; }
    public float StretchAmount {  get; set; }
    public int XAmplification { get; set; }
    public int YAmplification { get; set; }
    public float XFrequency { get; set; }
    public float YFrequency { get; set; }
    public byte ShowOnBlink { get; set; }
    public byte ShowOnTalk { get; set; }
    public byte ZLayer { get; set; }
}
