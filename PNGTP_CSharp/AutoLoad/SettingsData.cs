using Godot;
using Godot.Collections;

internal class SettingsData
{
    public bool NewUser { get; set; }
    public string LastAvatar { get; set; }
    public float Volume { get; set; }
    public float Sense {  get; set; }
    public int[] WindowSize { get; set; }
    public int Bounce { get; set; }
    public int Gravity { get; set; }
    public int MaxFps { get; set; }
    public int SecondsToMicReset { get; set; }
    public float[] BackgroundColor { get; set; }
    public bool Filtering { get; set; }
    public string[] CostumeKeys { get; set; }
    public float BlinkSpeed { get; set; }
    public int BlinkChance { get; set; }
    public bool BounceOnCostumeChange { get; set; }
}

