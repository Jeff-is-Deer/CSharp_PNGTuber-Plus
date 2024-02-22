using Godot;
using static GlobalClass;
using System;

public partial class Main : Node2D
{
	public Node2D Origin { get; set; } = null;
	public Node2D EditControls { get; set; } = null;
	public Node2D ControlPanel { get; set; } = null;
	public Node2D HowTo {  get; set; } = null;
	public Node2D SpriteList { get; set; } = null;
	public Node2D Lines { get; set; } = null;
	public float BounceChange { get; set; } = 0.0f;
	public bool EditMode { get; set; } = true;
	public bool IsFileSystemOpen { get; set; } = false;

	public int YVelocity { get; set; } = 0;
	public int BounceSlider { get; set; } = 250;
	public int BounceGravity { get; set; } = 1000;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.Main = this;
		Global.Failed = GetNode<Node2D>("Failed");
		Origin = GetNode<Node2D>("OriginMotion/Origin");
		EditControls = GetNode<Node2D>("EditControls");
		ControlPanel = GetNode<Node2D>("ControlPanel");
		HowTo = GetNode<Node2D>("HowTo");
		SpriteList = GetNode<Node2D>("SpriteList");
		Lines = GetNode<Node2D>("Lines");
        Global.Main = this;
        Global.Failed = GetNode<Node2D>("Failed");
        Global.StartSpeaking += OnSpeak;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void OnSpeak()
	{
		if(Origin.GetParent<Sprite2D>().Position.Y > -16) {
			YVelocity = BounceSlider * -1;
		}
	}

	public void SwapMode()
	{
		Global.HeldSprite = null;
		EditMode = !EditMode;
		if (Global.BackgroundColor.A != 0.0f) {
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
}
