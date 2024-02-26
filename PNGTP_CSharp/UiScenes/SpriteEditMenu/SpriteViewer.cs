using System;
using Godot;
using static GlobalClass;


public partial class SpriteViewer : Node2D
{
    public Sprite3D SpriteSpin { get; set; } = null;
    public Sprite3D ParentSpin { get; set; } = null;
    public Sprite2D SpriteRotationalDisplay { get; set; } = null;
    public CollisionShape2D CoverCollider { get; set; } = null;
    public override void _Ready()
    {
        base._Ready();
        SpriteSpin = GetNode<Sprite3D>("SubViewportContainer/SubViewport/Node3D/Sprite3D");
        ParentSpin = GetNode<Sprite3D>("SubViewportContainer2/SubViewport/Node3D/Sprite3D");
        SpriteRotationalDisplay = GetNode<Sprite2D>("RotationalLimits/RotBack/SpriteDisplay");
        CoverCollider = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
        Global.SpriteEdit = this;
    }

    public void SetImage()
    {
        if(Global.SelectedSprite == null) { return; }
        float spriteYLen = Global.SelectedSprite.ImageData.GetSize().Y;
        SpriteSpin.Texture = Global.SelectedSprite.Texture; // i think i may have overcomplicated the object structure.  may need complete rework.
        SpriteSpin.PixelSize = 1.5f / spriteYLen;
        SpriteSpin.Hframes = Global.SelectedSprite.Frames;
        SpriteRotationalDisplay.Texture = Global.SelectedSprite.Texture;
        SpriteRotationalDisplay.Offset = Global.SelectedSprite.Offset;
        SpriteRotationalDisplay.Scale = new Vector2(1.0f , 1.0f) * ( 150.0f / spriteYLen );

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
