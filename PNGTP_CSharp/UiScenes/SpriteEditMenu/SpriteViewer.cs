using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
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

    public TextureProgressBar RotationalLimitBar { get; set; } = null;

    public Label RotationalDragLabel { get; set; } = null;
    public HSlider RotationalDragSlider { get; set; } = null;

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

    public Sprite2D SpeakingButtonSprite { get; set; } = null;
    public Button SpeakingButton { get; set; } = null;

    public Sprite2D BlinkingButtonSprite { get; set; } = null;
    public Button BlinkingButton { get; set; } = null;

    public Sprite2D UnlinkSprite { get; set; } = null;
    public Button UnlinkButton { get; set; } = null;

    public Label PositionLabel { get; set; } = null;
    public Label OffsetLabel { get; set; } = null;
    public Label LayerLabel { get; set; } = null;

    public Sprite2D RotationalLine_3 { get; set; } = null;
    public Sprite2D RotationalLine_2 { get; set; } = null;
    public Sprite2D RotationalLine_1 { get; set; } = null;

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
        RotationalDragSlider = GetNode<HSlider>("Rotation/RotationalDragSlider");

        SpeakingButtonSprite = GetNode<Sprite2D>("Buttons/SpeakingSprite");
        SpeakingButton = GetNode<Button>("Buttons/SpeakingSprite/SpeakingButton");

        BlinkingButtonSprite = GetNode<Sprite2D>("Buttons/BlinkingSprite");
        BlinkingButton = GetNode<Button>("Buttons/BlinkingSprite/BlinkingButton");

        RotationalLimitMaximumLabel = GetNode<Label>("RotationalLimits/MaximumLabel");
        RotationalLimitMaximumSlider = GetNode<HSlider>("RotationalLimits/MaximumSlider");

        RotationalLimitMinimumLabel = GetNode<Label>("RotationalLimits/MinimumLabel");
        RotationalLimitMinimumSlider = GetNode<HSlider>("RotationalLimits/MinimumSlider");

        RotationalLimitBar = GetNode<TextureProgressBar>("RotationalLimits/RotateBack/RotationalLimitBar");

        FileTitleLabel = GetNode<Label>("Position/FileTitleLabel");

        IgnoreBounceCheckBox = GetNode<Button>("Buttons/IgnoreBounce");
        ClipLinkedCheckBox = GetNode<Button>("Buttons/ClipLinked");

        AnimationFramesLabel = GetNode<Label>("Animation/AnimationFramesLabel");
        AnimationFramesSlider = GetNode<HSlider>("Animation/AnimationFramesSlider");
        AnimationSpeedLabel = GetNode<Label>("Animation/AnimationSpeedLabel");
        AnimationSpeedSlider = GetNode<HSlider>("Animation/AnimationSpeedSlider");

        UnlinkSprite = GetNode<Sprite2D>("Buttons/UnlinkSprite");
        UnlinkButton = GetNode<Button>("Buttons/UnlinkSprite/UnlinkButton");

        PositionLabel = GetNode<Label>("PositionLabels/Position");
        OffsetLabel = GetNode<Label>("PositionLabels/Offset");
        LayerLabel = GetNode<Label>("PositionLabels/Layer");

        RotationalLine_1 = GetNode<Sprite2D>("RotationalLimits/RotateBack/Line1");
        RotationalLine_2 = GetNode<Sprite2D>("RotationalLimits/RotateBack/Line2");
        RotationalLine_3 = GetNode<Sprite2D>("RotationalLimits/RotateBack/Line3");





        if ( Global.SelectedAvatarPart.PartData.PID == 0 ) {
            UnlinkSprite.Visible = false;
        }

        Global.AvatarPartDetailEdit = this;
    }

    public override void _Process(double delta)
    {
        Visible = Global.SelectedAvatarPart != null;
        CoverCollider.Disabled = !Visible;

        if(!Visible) {
            return;
        }
        AvatarPartObject part = Global.SelectedAvatarPart;
        ChildPartSpin.RotateY((float)delta * 4.0f);
        ParentPartSpin.RotateY((float)delta * 4.0f);
        PositionLabel.Text = $"Position X: {part.PartData.Position.X}    Y: {part.PartData.Position.Y}";
        OffsetLabel.Text =   $"Offset   X: {part.PartData.Offset.X}    Y: {part.PartData.Offset.Y}";
        LayerLabel.Text =    $"Layer       {part.PartData.ZIndex}";
        int rotLimitMin = Global.SelectedAvatarPart.PartData.RotationalLimitMinimum;
        int rotationSize = Global.SelectedAvatarPart.PartData.RotationalLimitMaximum - rotLimitMin;
        SpriteRotationalDisplay.RotationDegrees = Mathf.Sin(Global.AnimationTick * 0.05f) * ( rotationSize / 2 ) + ( rotLimitMin + ( rotationSize / 2 ) );
        RotationalLine_3.RotationDegrees = SpriteRotationalDisplay.RotationDegrees;
    }

    public void SetImage()
    {
        if ( Global.SelectedAvatarPart == null ) { return; }
        float partHeight = Global.SelectedAvatarPart.ImageData.GetSize().Y;
        ChildPartSpin.Texture = Global.SelectedAvatarPart.PartData.Texture; // i think i may have overcomplicated the object structure.  may need complete rework.
        ChildPartSpin.PixelSize = 1.5f / partHeight;
        ChildPartSpin.Hframes = Global.SelectedAvatarPart.PartData.NumberOfFrames;
        SpriteRotationalDisplay.Texture = Global.SelectedAvatarPart.PartData.Texture;
        SpriteRotationalDisplay.Offset = Global.SelectedAvatarPart.PartData.Offset;
        SpriteRotationalDisplay.Scale = new Vector2(1.0f , 1.0f) * ( 150.0f / partHeight );

        DragSliderLabel.Text = $"Drag: {Global.SelectedAvatarPart.PartData.DragSpeed}";
        DragSlider.Value = Global.SelectedAvatarPart.PartData.DragSpeed;

        XWobbleFrequencyLabel.Text = $"X frequency: {Global.SelectedAvatarPart.PartData.XFrequency}";
        XWobbleAmplitudeLabel.Text = $"X amplitude: {Global.SelectedAvatarPart.PartData.XAmplitude}";
        XWobbleFrequencySlider.Value = Global.SelectedAvatarPart.PartData.XFrequency;
        XWobbleAmplitudeSlider.Value = Global.SelectedAvatarPart.PartData.XAmplitude;

        YWobbleFrequencyLabel.Text = $"Y frequency: {Global.SelectedAvatarPart.PartData.YFrequency}";
        YWobbleAmplitudeLabel.Text = $"Y amplitude: {Global.SelectedAvatarPart.PartData.YAmplitude}";
        YWobbleFrequencySlider.Value = Global.SelectedAvatarPart.PartData.YFrequency;
        YWobbleAmplitudeSlider.Value = Global.SelectedAvatarPart.PartData.YAmplitude;

        RotationalDragLabel.Text = $"Rotational drag: {Global.SelectedAvatarPart.PartData.RotationalDragStrength}";
        RotationalDragSlider.Value = Global.SelectedAvatarPart.PartData.RotationalDragStrength;

        SpeakingButtonSprite.Frame = Global.SelectedAvatarPart.PartData.ShowOnTalk;
        BlinkingButtonSprite.Frame = Global.SelectedAvatarPart.PartData.ShowOnBlink;

        RotationalLimitMaximumLabel.Text = $"Rotational limit max: {Global.SelectedAvatarPart.PartData.RotationalLimitMaximum}";
        RotationalLimitMaximumSlider.Value = Global.SelectedAvatarPart.PartData.RotationalLimitMaximum;
        RotationalLimitMinimumLabel.Text = $"Rotational limit min: {Global.SelectedAvatarPart.PartData.RotationalLimitMinimum}";
        RotationalLimitMinimumSlider.Value = Global.SelectedAvatarPart.PartData.RotationalLimitMinimum;

        SquishLabel.Text = $"Squish: {Global.SelectedAvatarPart.PartData.StretchAmount}";
        SquishSlider.Value = Global.SelectedAvatarPart.PartData.StretchAmount;

        FileTitleLabel.Text = Global.SelectedAvatarPart.PartData.FilePath;

        ClipLinkedCheckBox.ButtonPressed = Global.SelectedAvatarPart.PartData.IsClipped;
        IgnoreBounceCheckBox.ButtonPressed = Global.SelectedAvatarPart.PartData.IgnoresBounce;

        AnimationSpeedLabel.Text = $"Animation speed: {Global.SelectedAvatarPart.PartData.AnimationSpeed}";
        AnimationSpeedSlider.Value = Global.SelectedAvatarPart.PartData.AnimationSpeed;

        AnimationFramesLabel.Text = $"Animation frames: {Global.SelectedAvatarPart.PartData.NumberOfFrames}";
        AnimationFramesSlider.Value = Global.SelectedAvatarPart.PartData.NumberOfFrames;

        ChangeRotationalLimit();
        SetLayerButtons();
        HasParent();



    }
    public void ChangeRotationalLimit()
    {
        RotationalLimitBar.Value = Global.SelectedAvatarPart.PartData.RotationalLimitMaximum - Global.SelectedAvatarPart.PartData.RotationalLimitMinimum;
        RotationalLimitBar.RotationDegrees = Global.SelectedAvatarPart.PartData.RotationalLimitMinimum + 90;
        RotationalLine_1.RotationDegrees = Global.SelectedAvatarPart.PartData.RotationalLimitMinimum;
        RotationalLine_2.RotationDegrees = Global.SelectedAvatarPart.PartData.RotationalLimitMaximum;
    }

    public void SetLayerButtons()
    {
        byte[] visibleOnLayers = Global.SelectedAvatarPart.PartData.VisibleOnCostumeLayer;
        GetNode<Sprite2D>("LayerButtons/Layer1").Frame = visibleOnLayers[0];
        GetNode<Sprite2D>("LayerButtons/Layer2").Frame = visibleOnLayers[1];
        GetNode<Sprite2D>("LayerButtons/Layer3").Frame = visibleOnLayers[2];
        GetNode<Sprite2D>("LayerButtons/Layer4").Frame = visibleOnLayers[3];
        GetNode<Sprite2D>("LayerButtons/Layer5").Frame = visibleOnLayers[4];
        GetNode<Sprite2D>("LayerButtons/Layer6").Frame = visibleOnLayers[5];
        GetNode<Sprite2D>("LayerButtons/Layer7").Frame = visibleOnLayers[6];
        GetNode<Sprite2D>("LayerButtons/Layer8").Frame = visibleOnLayers[7];
        GetNode<Sprite2D>("LayerButtons/Layer9").Frame = visibleOnLayers[8];
        GetNode<Sprite2D>("LayerButtons/Layer10").Frame = visibleOnLayers[9];
        List<AvatarPartObject> savedParts = GetTree().GetNodesInGroup("saved").OfType<AvatarPartObject>().ToList();
        foreach (AvatarPartObject part in savedParts) {
            if ( part.PartData.VisibleOnCostumeLayer[Global.Main.] ) {

            }
        }
    }

    public void HasParent()
    {
        if ( Global.SelectedAvatarPart.PartData.PID == 0 ) {
            UnlinkButton.Visible = false;
            ParentPartSpin.Visible = false;
        }
        else {
            UnlinkButton.Visible = true;
            try {
                AvatarPartObject parentPart = GetTree().GetNodesInGroup(Global.SelectedAvatarPart.PartData.PID.ToString()).OfType<AvatarPartObject>().ToList()[0];
                ParentPartSpin.Texture = parentPart.PartData.Texture;
                ParentPartSpin.PixelSize = 1.5f / parentPart.ImageData.GetSize().Y;
                ParentPartSpin.Hframes = parentPart.PartData.NumberOfFrames;
                ParentPartSpin.Visible = true;
            }
            catch {
                return;
            }
        }
    }
}
