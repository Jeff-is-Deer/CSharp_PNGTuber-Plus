public class SpriteData
{
    public string Path { get; set; }
    public byte Type { get; set; }
    public float[] Offset { get; set; } = new float[2];
    public float[] Position { get; set; } = new float[2];
    public int DragSpeed { get; set; }
    public byte Identification { get; set; }
    public byte ParentIdentification { get; set; }
    public int RotationalDragStrength { get; set; }
    // Sprite rendering
    public byte ShowOnBlink { get; set; }
    public byte ShowOnTalk { get; set; }
    // Wobble
    public float XFrequency { get; set; }
    public float YFrequency { get; set; }
    public float XAmplification { get; set; }
    public float YAmplification { get; set; }
    // Layers
    public byte ZLayer { get; set; }
}

