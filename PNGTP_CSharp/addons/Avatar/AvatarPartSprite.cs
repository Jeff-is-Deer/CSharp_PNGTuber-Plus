using System;
using Godot;
using Newtonsoft.Json;
public partial class AvatarPartSprite : Sprite2D
{
    public byte ID { get; set; }
    public byte PID { get; set; }
    public AvatarPartSprite ParentPart { get; set; }
    public byte[] Children { get; set; }
    public string FilePath { get; set; }
    public byte Type { get; set; } // Enum Type
    public string Base64ImageData { get; set; }
    public int AnimationSpeed { get; set; }
    public bool IsClipped { get; set; }
    public bool IgnoresBounce { get; set; }
    public int[] VisibleOnCostumeLayer { get; set; } // What costume this sprite is visible on
    public int NumberOfFrames { get; set; }
    public int RotationalLimitMaximum { get; set; }
    public int RotationalLimitMinimum { get; set; }
    public int DragSpeed { get; set; }
    public int RotationalDragStrength { get; set; }
    public float SquishAmount { get; set; }
    public int XAmplitude { get; set; }
    public int YAmplitude { get; set; }
    public float XFrequency { get; set; }
    public float YFrequency { get; set; }
    public byte ShowOnBlink { get; set; }
    public byte ShowOnTalk { get; set; }

    public class jsonAvatarPartSprite
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
        public int RotationalLimitMaximum { get; set; }
        public int RotationalLimitMinimum { get; set; }
        public int DragSpeed { get; set; }
        public int RotationalDragStrength { get; set; }
        public float SquishAmount { get; set; }
        public int XAmplitude { get; set; }
        public int YAmplitude { get; set; }
        public float XFrequency { get; set; }
        public float YFrequency { get; set; }
        public byte ShowOnBlink { get; set; }
        public byte ShowOnTalk { get; set; }
        public byte ZLayer { get; set; }
    }

}
