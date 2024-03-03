using Godot;
using System;
using static GlobalClass;

public partial class Main : Node2D
{
    public PackedScene SpriteObject { get; set; } = ResourceLoader.Load<PackedScene>("res://UiScenes/SelectedSprite/SpriteObject.tscn");
    public Avatar UserAvatar { get; set; }
    public Node2D Origin { get; set; } = null;
    public Node2D EditControls { get; set; } = null;
    public Node2D ControlPanel { get; set; } = null;
    public Node2D HowTo { get; set; } = null;
    public Node2D SpriteList { get; set; } = null;
    public Node2D Lines { get; set; } = null;
    public FileDialog FileDialog { get; set; } = null;
    public FileDialog ReplaceDialog { get; set; } = null;
    public FileDialog SaveDialog { get; set; } = null;
    public FileDialog LoadDialog { get; set; } = null;
    public HSlider MicrophoneVolumeSlider { get; set; } = null;
    public HSlider MicrophoneSensitivitySlider { get; set; } = null;
    public byte SelectedCostume { get; set; } = 0;
    public float BounceChange { get; set; } = 0.0f;
    public bool EditMode { get; set; } = true;
    public bool IsFileSystemOpen { get; set; } = false;

    public int YVelocity { get; set; } = 0;
    public int BounceSlider { get; set; } = 250;
    public int BounceGravity { get; set; } = 1000;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.Failed = GetNode<Node2D>("Failed");
        Origin = GetNode<Node2D>("OriginMotion/Origin");
        EditControls = GetNode<Node2D>("EditControls");
        ControlPanel = GetNode<Node2D>("ControlPanel");
        HowTo = GetNode<Node2D>("HowTo");
        SpriteList = GetNode<Node2D>("EditControls/SpriteList");
        Lines = GetNode<Node2D>("Lines");

        FileDialog = GetNode<FileDialog>("FileDialog");
        ReplaceDialog = GetNode<FileDialog>("ReplaceDialog");
        SaveDialog = GetNode<FileDialog>("SaveDialog");
        LoadDialog = GetNode<FileDialog>("LoadDialog");

        UserAvatar = GetNode<Avatar>("OriginMotion/Origin/Avatar");

        MicrophoneVolumeSlider = GetNode<HSlider>("ControlPanel/MicrophoneVolumeSlider");
        MicrophoneSensitivitySlider = GetNode<HSlider>("ControlPanel/MicrophoneSensitivitySlider");


        Global.Main = this;
        Global.StartSpeaking += OnSpeak;

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

    public void OnSpeak()
    {
        if ( Origin.GetParent<Node2D>().Position.Y > -16 ) {
            YVelocity = BounceSlider * -1;
        }
    }

    public void SwapMode()
    {
        Global.SelectedSprite = null;
        EditMode = !EditMode;
        if ( Global.BackgroundColor.A != 0.0f ) {
            GetViewport().TransparentBg = false;
        }
        RenderingServer.SetDefaultClearColor(Global.BackgroundColor);
        EditControls.SetProcess(EditMode);
        ControlPanel.SetProcess(!EditMode);
        EditControls.Visible = EditMode;
        HowTo.Visible = EditMode;
        ControlPanel.Visible = EditMode;
        Lines.Visible = EditMode;
        SpriteList.Visible = EditMode;
    }
    public void AddImage(string path)
    {
        AvatarPartObject sprite = SpriteObject.Instantiate<AvatarPartObject>();
        PartData newSprite = new PartData();
        //newSprite.Path = path;
        //newSprite.Identification = 
    }

    #region Event responses

    public void Event_AddButtonPressed()
    {
        FileDialog.Visible = true;
    }

    public void Event_FileDialogFileSelected(object sender , EventArgs e)
    {
        AddImage(e.Path);
    }
    public void Event_LinkButtonPressed()
    {
        Global.ReparentingMode = true;
        Global.Chain.Enable(Global.ReparentingMode);
        Global.PushUpdate("Linking sprites!");
    }
    #endregion
}
