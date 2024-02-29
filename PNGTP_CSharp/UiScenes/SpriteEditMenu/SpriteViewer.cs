using System;
using Godot;
using static GlobalClass;


public partial class AvatarPartDetails : Node2D
{
    public Sprite3D ChildPartSpin { get; set; } = null;
    public Sprite3D ParentPartSpin { get; set; } = null;
    public Sprite2D SpriteRotationalDisplay { get; set; } = null;
    public CollisionShape2D CoverCollider { get; set; } = null;


    /* ----------------------------------- ^/
     * "xFreq"    -> "XWobbleFrequency"
     * "xAmp"     -> "XWobbleAmplitude"
     * 
     * "yFreq"    -> "YWobbleFrequency"
     * "yAmp"     -> "YWobbleAmplitude"
     * 
     * "rDrag"    -> "RotationalDrag"
     * "rotLimit" -> "RotationalLimit"
    /^ ----------------------------------- */
    public Label DragSliderLabel { get; set; } = null;
    public HSlider DragSlider { get; set; } = null;

    public Label XWobbleFrequencyLabel { get; set; } = null;
    public HSlider XWobbleFrequencySlider { get; set; } = null;

    public Label XWobbleAmplitudeLabel { get; set; } = null;
    public HSlider XWobbleAmplitudeSlider { get; set; } = null;

    public Label YWobbleFrequencyLabel { get; set; } = null;
    public HSlider YWobbleFrequencySlider { get; set; } = null;

    public Label YWobbleAmplitudeLabel { get; set; } = null;
    public HSlider YWobbleAmplitudeSlider { get; set; } = null;

    public Label RotationalDragLabel { get; set; } = null;
    public HSlider RotatiaonalDragSlider { get; set; } = null;

    public Label RotationalLimitMaximumLabel { get; set; } = null;
    public HSlider RotationalLimitMaximumSlider { get; set; } = null;

    public Label RotationalLimitMinimumLabel { get; set; } = null;
    public HSlider RotationalLimitMinimumSlider { get; set; } = null;

    public Label SquishLabel { get; set; } = null;
    public HSlider SquishSlider { get; set; } = null;

    public Label AnimationSpeedLabel { get; set; } = null;
    public HSlider AnimationSpeedSlider { get; set; } = null;

    public Label AnimationFramesLabel { get; set; } = null;
    public HSlider AnimationFramesSlider { get; set; } = null;

    public Label FileTitleLabel { get; set; } = null;

    public Button IgnoreBounceCheckBox { get; set; } = null;
    public Button ClipLinkedCheckBox { get; set; } = null;

    public Sprite2D SpeakingSprite { get; set; } = null;
    public Button SpeakingButton { get; set; } = null;

    public Sprite2D BlinkingSprite { get; set; } = null;
    public Button BlinkingButton { get; set; } = null;

    public Sprite2D UnlinkSprite { get; set; } = null;
    public Button UnlinkButton { get; set; } = null;

    public override void _Ready()
    {
        ChildPartSpin = GetNode<Sprite3D>("SubViewportContainer/SubViewport/Node3D/ChildPart3D"); // originally 'SubViewportContainer/SubViewport/Node3D/Sprite3D'
        ParentPartSpin = GetNode<Sprite3D>("SubViewportContainer2/SubViewport/Node3D/ParentPart3D"); // originally 'SubViewportContainer2/SubViewport/Node3D/Sprite3D'
        SpriteRotationalDisplay = GetNode<Sprite2D>("RotationalLimits/RotBack/SpriteDisplay");
        CoverCollider = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
        DragSliderLabel = GetNode<Label>("Drag/DragSliderLabel"); // originally 'Slider/Label'
        DragSlider = GetNode<HSlider>("Slider/DragSlider"); // originally 'Slider/DragSlider'

        XWobbleFrequencyLabel = GetNode<Label>("WobbleControl/XWobbleFrequencyLabel");
        XWobbleFrequencySlider = GetNode<HSlider>("WobbleControl/XWobbleFrequecySlider");
        XWobbleAmplitudeLabel = GetNode<Label>("WobbleControl/XWobbleAmplitudeLabel");
        XWobbleAmplitudeSlider = GetNode<HSlider>("WobbleControl/XWobbleAmplitudeSlider");

        YWobbleFrequencyLabel = GetNode<Label>("WobbleControl/YWobbleFrequencyLabel");
        YWobbleFrequencySlider = GetNode<HSlider>("WobbleControl/YWobbleFrequecySlider");
        YWobbleAmplitudeLabel = GetNode<Label>("WobbleControl/YWobbleAmplitudeLabel");
        YWobbleAmplitudeSlider = GetNode<HSlider>("WobbleControl/YWobbleAmplitudeSlider");

        RotationalDragLabel = GetNode<Label>("Rotation/RotationalDragLabel");
        RotatiaonalDragSlider = GetNode<HSlider>("Rotation/RotationalDragSlider");

        SpeakingSprite = GetNode<Sprite2D>("Buttons/SpeakingSprite");
        SpeakingButton = GetNode<Button>("Buttons/SpeakingSprite/SpeakingButton");

        BlinkingSprite = GetNode<Sprite2D>("Buttons/BlinkingSprite");
        BlinkingButton = GetNode<Button>("Buttons/BlinkingSprite/BlinkingButton");

        RotationalLimitMaximumLabel = GetNode<Label>("RotationalLimits/MaximumLabel");
        RotationalLimitMaximumSlider = GetNode<HSlider>("RotationalLimits/MaximumSlider");

        RotationalLimitMinimumLabel = GetNode<Label>("RotationalLimits/MinimumLabel");
        RotationalLimitMinimumSlider = GetNode<HSlider>("RotationalLimits/MinimumSlider");

        FileTitleLabel = GetNode<Label>("Position/FileTitleLabel");

        IgnoreBounceCheckBox = GetNode<Button>("Buttons/IgnoreBounce");
        ClipLinkedCheckBox = GetNode<Button>("Buttons/ClipLinked");

        AnimationFramesLabel = GetNode<Label>("Animation/AnimationFramesLabel");
        AnimationFramesSlider = GetNode<HSlider>("Animation/AnimationFramesSlider");
        AnimationSpeedLabel = GetNode<Label>("Animation/AnimationSpeedLabel");
        AnimationSpeedSlider = GetNode<HSlider>("Animation/AnimationSpeedSlider");

        UnlinkSprite = GetNode<Sprite2D>("Buttons/UnlinkSprite");
        UnlinkButton = GetNode<Button>("Buttons/UnlinkSprite/UnlinkButton");

        ChangeRotationalLimit();
        SetLayerButtons();

        if (Global.SelectedAvatarPart.PartData.PID == 0) {
            UnlinkSprite.Visible = false;
        }

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

        DragSliderLabel.Text = $"Drag: {Global.SelectedAvatarPart.PartData.DragSpeed}";
        DragSlider.Value = Global.SelectedAvatarPart.PartData.DragSpeed;





    }
}
