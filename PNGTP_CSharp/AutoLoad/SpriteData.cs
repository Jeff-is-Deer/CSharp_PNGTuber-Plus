using Godot;
using System;

public class SpriteData : AvatarData
{
    public int Drag { get; set; }
    public int Identification { get; set; }
    public float[] Offset { get; set; } = new float[2];
    public int ParentId { get; set; }
    public string PathLocation {  get; set; }
    public float[] Position { get; set; } = new float[2];
    public int RotationalDrag { get; set; }
    public int ShowBlink { get; set; }
    public int ShowTalk { get; set; }
    public float XAmplification { get; set; }
    public float YAmplification { get; set; }
    public float XFrequency { get; set; }
    public float YFrequency { get; set; }
    public int ZLayer { get; set; }
}

