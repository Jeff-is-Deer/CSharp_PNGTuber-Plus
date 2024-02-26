using Godot;
using System;
using static GlobalClass;

public partial class SpriteListObject : NinePatchRect
{
    public Sprite2D SpritePreview { get; set; } = null;
    public Sprite2D Outline { get; set; } = null;
    public Sprite2D Fade { get; set; } = null;
    public Label SpriteLabel { get; set; } = null;
    public Line2D Line { get; set; } = null;
    public SpriteObject Sprite { get; set; } = null;
    public string SpritePath { get; set; } = string.Empty;
    public int IndentLayer { get; set; } = 0;


    public override void _Ready()
    {
        Sprite = new SpriteObject();
        SpritePreview = GetNode<Sprite2D>("SpritePreview/Sprite2D");
        Outline = GetNode<Sprite2D>("Selected");
        Fade = GetNode<Sprite2D>("Fade");
        SpriteLabel = GetNode<Label>("Label");
        Line = GetNode<Line2D>("Line2D");
        byte spriteCount = (byte)SpritePath.Split('/').Length;
        SpriteLabel.Text = SpritePath.Split('/')[spriteCount];
        Line.Visible = false;
    }

    public void _Process()
    {
        Outline.Visible = Sprite == Global.SelectedSprite;
    }
    public void UpdateVisibility()
    {
        Fade.Visible = !SpritePreview.Visible;
    }
    public void UpdateChildren()
    {
    }

    public void Event_ButtonPressed()
    {
        if(Global.SelectedSprite != null && Global.ReparentingMode) {
            //Global.LinkSprite(Global.SelectedSprite , Sprite);
            Global.Chain.Enable(true);
        }

    }
}
