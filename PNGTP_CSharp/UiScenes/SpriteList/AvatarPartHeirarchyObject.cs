using Godot;
using System;
using static GlobalClass;

public partial class AvatarPartHeirarchyObject : NinePatchRect
{
    public Sprite2D AvatarPartPreview { get; set; } = null;
    public Sprite2D SelectedOutline { get; set; } = null;
    public Sprite2D ShadeFilter { get; set; } = null;
    public Label AvatarPartNameLabel { get; set; } = null;
    public Line2D HeirarchyLine { get; set; } = null;
    public Button PreviewButton { get; set; } = null;
    public AvatarPartSprite AvatarPart { get; set; } = null;
    public string AvatarPartPath { get; set; } = string.Empty;
    public int IndentLayer { get; set; } = 0;


    public override void _Ready()
    {
        AvatarPartPreview = GetNode<Sprite2D>("PartPreview/Part"); // originally 'SpritePreview/Sprite2D'
        SelectedOutline = GetNode<Sprite2D>("SelectedOutline"); // originally 'Selected'
        ShadeFilter = GetNode<Sprite2D>("ShadeFilter"); // originally 'Fade'
        AvatarPartNameLabel = GetNode<Label>("PartName"); // originally 'Label'
        HeirarchyLine = GetNode<Line2D>("HeirarchyLine"); // originally 'Line2D'
        int partNameIndex = AvatarPartPath.Split('/').Length - 1;
        AvatarPartNameLabel.Text = AvatarPartPath.Split('/')[partNameIndex];
        HeirarchyLine.Visible = false;
    }

    public void _Process()
    {
        SelectedOutline.Visible = AvatarPart == Global.SelectedAvatarPart.SpriteData;
    }
    public void UpdateVisibility()
    {
        ShadeFilter.Visible = !AvatarPartPreview.Visible;
    }
    public void UpdateChildren()
    {
    }


    public void Event_ButtonPressed()
    {
        if(Global.SelectedAvatarPart != null && Global.ReparentingMode) {
            //Global.LinkSprite(Global.SelectedSprite , Sprite);
            Global.Chain.Enable(true);
        }

    }
}
