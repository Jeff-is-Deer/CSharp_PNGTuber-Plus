using System;
using System.Linq;
using Godot;
using static GlobalClass;


public partial class AvatarPartDetails : Node2D
{
    public class LayerButton
    {
        public Sprite2D sprite { get; set; }
        public Button button { get; set; }
    }

    #region signals
    [Signal]
    public delegate void DragSliderValueChangedEventHandler(int value);
    [Signal]
    public delegate void XFrequencyValueChangedEventHandler(float value);
    [Signal]
    public delegate void XAmplitudeValueChangedEventHandler(int value);
    [Signal]
    public delegate void YFrequencyValueChangedEventHandler(float value);
    [Signal]
    public delegate void YAmplitudeValueChangedEventHandler(int value);
    [Signal]
    public delegate void RotationalDragValueChangedEventHandler(int value);
    [Signal]
    public delegate void RotationalLimitMinimumValueChangedEventHandler(int value);
    [Signal]
    public delegate void RotationalLimitMaximumValueChangedEventHandler(int value);
    [Signal]
    public delegate void SquishValueChangedEventHandler(int value);
    [Signal]
    public delegate void AnimationSpeedValueChangedEventHandler(int value);
    [Signal]
    public delegate void AnimationFramesValueChangedEventHandler(int value);
    [Signal]
    public delegate void LayerButtonPressedEventHandler(int layer);
    #endregion
    #region nodes
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

    public LayerButton[] LayerButtons { get; set; } = null;
    public Sprite2D ButtonOutline { get; set; } = null;
    #endregion

    #region methods
    public override void _Ready()
    {

        DragSliderValueChanged += event_DragSliderValueChanged;
        XFrequencyValueChanged += event_XFreqencyValueChanged;
        XAmplitudeValueChanged += event_XAmplitudeValueChanged;
        YFrequencyValueChanged += event_YFreqencyValueChanged;
        YAmplitudeValueChanged += event_YAmplitudeValueChanged;
        RotationalDragValueChanged += event_RotationalDragValueChanged;
        RotationalLimitMaximumValueChanged += event_RotationalLimitMaximumValueChanged;
        RotationalLimitMinimumValueChanged += event_RotationalLimitMinimumValueChanged;
        SquishValueChanged += event_SquishValueChanged;
        AnimationSpeedValueChanged += event_AnimationSpeedValueChanged;
        AnimationFramesValueChanged += event_AnimationFramesValueChanged;
        LayerButtonPressed += event_LayerButtonPressed;


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

        ButtonOutline = GetNode<Sprite2D>("LayerButtons/Outline");

        Global.AvatarPartDetailEdit = this;
    }

    public override void _Process(double delta)
    {
        Visible = Global.SelectedAvatarPart != null;
        CoverCollider.Disabled = !Visible;

        if(!Visible) {
            return;
        }
        ChildPartSpin.RotateY((float)delta * 4.0f);
        ParentPartSpin.RotateY((float)delta * 4.0f);
        PositionLabel.Text = $"Position X: {Global.SelectedAvatarPart.PartData.Position.X}    Y: {Global.SelectedAvatarPart.PartData.Position.Y}";
        OffsetLabel.Text =   $"Offset   X: {Global.SelectedAvatarPart.PartData.Offset.X}    Y: {Global.SelectedAvatarPart.PartData.Offset.Y}";
        LayerLabel.Text =    $"Layer       {Global.SelectedAvatarPart.PartData.ZIndex}";
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

        SquishLabel.Text = $"Squish: {Global.SelectedAvatarPart.PartData.SquishAmount}";
        SquishSlider.Value = Global.SelectedAvatarPart.PartData.SquishAmount;

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
        int[] visibleOnLayers = Global.SelectedAvatarPart.PartData.VisibleOnCostumeLayer;
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
        foreach (AvatarPartObject part in GetTree().GetNodesInGroup("saved").OfType<AvatarPartObject>().ToList()) {
            if ( part.PartData.VisibleOnCostumeLayer[Global.Main.SelectedCostume] == 1 ) {
                part.Visible = true;
                part.ChangeCollision(true);
            }
            else {
                part.Visible = false;
                part.ChangeCollision(false);
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
    public void LayerSelected(int layer)
    {
        ButtonOutline.Position = LayerButtons[layer].sprite.Position;
    }
    #endregion

    #region events
    public void event_DragSliderValueChanged(int value)
    {
        if (Global.SelectedAvatarPart != null) {
            DragSliderLabel.Text = $"Drag: {value}";
            Global.SelectedAvatarPart.PartData.DragSpeed = value;
        }
    }
    public void event_XFreqencyValueChanged(float value)
    {
        XWobbleFrequencyLabel.Text = $"X frequency: {value}";
        Global.SelectedAvatarPart.PartData.XFrequency = value;
    }
    public void event_XAmplitudeValueChanged(int value)
    {
        XWobbleAmplitudeLabel.Text = $"X amplitude: {value}";
        Global.SelectedAvatarPart.PartData.XAmplitude = value;
    }
    public void event_YFreqencyValueChanged(float value)
    {
        YWobbleFrequencyLabel.Text = $"Y frequency: {value}";
        Global.SelectedAvatarPart.PartData.YFrequency = value;
    }
    public void event_YAmplitudeValueChanged(int value)
    {
        YWobbleAmplitudeLabel.Text = $"Y amplitude: {value}";
        Global.SelectedAvatarPart.PartData.YAmplitude = value;
    }
    public void event_RotationalDragValueChanged(int value)
    {
        RotationalDragLabel.Text = $"Rotational drag: {value}";
        Global.SelectedAvatarPart.PartData.RotationalDragStrength = value;
    }
    public void event_RotationalLimitMinimumValueChanged(int value)
    {
        RotationalLimitMinimumLabel.Text = $"Rotational limit min: {value}";
        Global.SelectedAvatarPart.PartData.RotationalLimitMinimum = value;
        ChangeRotationalLimit();
    }
    public void event_RotationalLimitMaximumValueChanged(int value)
    {
        RotationalLimitMaximumLabel.Text = $"Rotational limit max: {value}";
        Global.SelectedAvatarPart.PartData.RotationalLimitMaximum = value;
        ChangeRotationalLimit();
    }
    public void event_SquishValueChanged(int value)
    {
        SquishLabel.Text = $"Squish: {value}";
        Global.SelectedAvatarPart.PartData.SquishAmount = value;
    }
    public void event_AnimationSpeedValueChanged(int value)
    {
        AnimationSpeedLabel.Text = $"Animation speed: {value}";
        Global.SelectedAvatarPart.PartData.AnimationSpeed = value;
    }
    public void event_AnimationFramesValueChanged(int value)
    {
        AnimationFramesLabel.Text = $"Sprite frames: {value}";
        Global.SelectedAvatarPart.PartData.NumberOfFrames = value;
        ChildPartSpin.Hframes = Global.SelectedAvatarPart.PartData.NumberOfFrames;
        Global.SelectedAvatarPart.ChangeFrames();
    }
    public void event_SpeakingButtonPressed()
    {
        SpeakingButtonSprite.Frame = ( SpeakingButtonSprite.Frame + 1 ) % 3;
        Global.SelectedAvatarPart.PartData.ShowOnTalk = (byte) SpeakingButtonSprite.Frame;
    }
    public void event_BlinkingButtonPressed()
    {
        BlinkingButtonSprite.Frame = ( BlinkingButtonSprite.Frame + 1 ) % 3;
        Global.SelectedAvatarPart.PartData.ShowOnBlink = (byte) BlinkingButtonSprite.Frame;
    }
    public void event_TrashButtonPressed()
    {
        Global.SelectedAvatarPart.QueueFree();
        Global.SelectedAvatarPart = null;
        Global.AvatarPartList.UpdateData();
    }
    public void event_UnlinkButtonPressed()
    {
        if ( Global.SelectedAvatarPart.PartData.PID == 0 ) {
            return;
        }
        Global.UnlinkAvatarPart();
        SetImage();
    }
    public void event_LayerButtonPressed(int layer)
    {
        Global.SelectedAvatarPart.PartData.VisibleOnCostumeLayer[layer] = Global.SelectedAvatarPart.PartData.VisibleOnCostumeLayer[layer] == 0 ? 1 : 0;
        ButtonOutline.Position = LayerButtons[layer].sprite.Position;
    }
    public void event_IgnoreBounceToggled(bool toggle)
    {
        Global.SelectedAvatarPart.PartData.IgnoresBounce = toggle;
    }
    public void event_ClipLinkToggled(bool toggle)
    {
        Global.SelectedAvatarPart.SetClip(toggle);
    }
    #endregion

    private LayerButton[] InitiateLayerButtons()
    {
        LayerButton[] buttons = new LayerButton[10];
        for (int i = 0; i < 10 ; i++) {
            buttons[i] = new LayerButton() {
                sprite = GetNode<Sprite2D>($"LayerButtons/Sprite{i}") ,
                button = GetNode<Button>($"LayerButtons/Sprite{i}/Button{i}")
            };
        }
        return buttons;
    }
}
