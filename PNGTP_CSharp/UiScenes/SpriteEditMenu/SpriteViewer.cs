using System;
using Godot;
using static GlobalClass;


public partial class AvatarPartDetails : Node2D
{
    public Sprite3D ChildPartSpin { get; set; } = null;
    public Sprite3D ParentPartSpin { get; set; } = null;
    public Sprite2D SpriteRotationalDisplay { get; set; } = null;
    public CollisionShape2D CoverCollider { get; set; } = null;
    public override void _Ready()
    {
        ChildPartSpin = GetNode<Sprite3D>("SubViewportContainer/SubViewport/Node3D/ChildPart3D"); // originally 'SubViewportContainer/SubViewport/Node3D/Sprite3D'
        ParentPartSpin = GetNode<Sprite3D>("SubViewportContainer2/SubViewport/Node3D/ParentPart3D"); // originally 'SubViewportContainer2/SubViewport/Node3D/Sprite3D'
        SpriteRotationalDisplay = GetNode<Sprite2D>("RotationalLimits/RotBack/SpriteDisplay");
        CoverCollider = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
        Global.AvatarPartDetailEdit = this;
    }

    public void SetImage()
    {
        if(Global.SelectedAvatarPart == null) { return; }
        float partHeight = Global.SelectedAvatarPart.ImageData.GetSize().Y;
        ChildPartSpin.Texture = Global.SelectedAvatarPart.PartData.Texture; // i think i may have overcomplicated the object structure.  may need complete rework.
        ChildPartSpin.PixelSize = 1.5f / partHeight;
        ChildPartSpin.Hframes = Global.SelectedAvatarPart.PartData.NumberOfFrames;
        SpriteRotationalDisplay.Texture = Global.SelectedAvatarPart.PartData.Texture;
        SpriteRotationalDisplay.Offset = Global.SelectedAvatarPart.PartData.Offset;
        SpriteRotationalDisplay.Scale = new Vector2(1.0f , 1.0f) * ( 150.0f / partHeight );

        /* ----------------------------------- ^/
         * NODE RENAMING STRUCTURE (for clarity):
         * "xFreq"    -> "XWobbleFrequency"
         * "xAmp"     -> "XWobbleAmplitude"
         * 
         * "yFreq"    -> "YWobbleFrequency"
         * "yAmp"     -> "YWobbleAmplitude"
         * 
         * "rDrag"    -> "RotationalDrag"
         * "rotLimit" -> "RotationalLimit"
        /^ ----------------------------------- */



    }
}
