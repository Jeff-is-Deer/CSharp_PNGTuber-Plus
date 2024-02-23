// TRANSLATED

using Godot;
using static GlobalClass;
using System;

public partial class SpriteObject : Sprite2D
{
	public const byte SPRITE = 1;
	public const byte ANIMATION = 2;
	// Nodes
	public PackedScene OutlineScene { get; set; } = ResourceLoader.Load<PackedScene>("res://UiScenes/SelectedSprite/Outline.tscn");
	public Sprite2D Sprite { get; set; } = null;
	public Sprite2D OriginSprite { get; set; } = null;
	public Area2D GrabArea { get; set; } = null;
	public Node2D DragOrigin { get; set; } = null;
	public Node2D Dragger { get; set; } = null;
	public Node2D WobbleOrigin { get; set; } = null;
	public SpriteData LoadedSprite { get; set; } = null;


	// Passed Variables
	public Image ImageData { get; set; } = null;
	public string LoadedImageData { get; set; } = null;
	public ImageTexture ImgTexture { get; set; } = null;

	public Sprite2D ParentSprite { get; set; } = null;
	public Vector2 ImageSize { get; set; } = Vector2.Zero;

	// Visuals
	public Vector2 MouseOffset { get; set; } = Vector2.Zero;
	public int GrabDelay { get; set; } = 0;
	public Vector2 Size { get; set; } = new Vector2(1 , 1);

	// Movement
	public int HeldTicks { get; set; } = 0;

	// Origin
	public int OriginTick { get; set; } = 0;


	// Rotational drag
	public Int16 RotationaLimitMax { get; set; } = 180;
	public Int16 RotationaLimitMin { get; set; } = -180;

	// Layer
	public byte[] CostumeLayers { get; set; } = { 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 };

	// Stretch
	public float StretchAmount { get; set; } = 0.0f;

	// Ignore bounce
	public bool IgnoreBounce { get; set; } = false;

	// Animation
	public int Frames { get; set; } = 1;
	public int AnimationSpeed { get; set; } = 0;
	public bool RemadePolygon { get; set; } = false;
	public bool IsClipped { get; set; } = false;
	public int Tick { get; set; } = 0;

	#region SpriteData
	/*
	public string Path { get; set; } = string.Empty;
    public byte Type { get; set; } = SPRITE;
    public int DragSpeed { get; set; } = 0;
    public byte Identification { get; set; } = 0;
    public byte ParentIdentification { get; set; } = 0;
//  public Vector2 Offset { get; set; } = new Vector2(SpriteData.Offset[0],SpriteData.Offset[1]);
//  public Vector2 Position { get; set; } = new Vector2)SpriteData.Position[0],SpriteData.Position[1]);
    public int RotationalDragStrength { get; set; } = 0;
    public byte ShowOnBlink { get; set; } = 0;
    public byte ShowOnTalk { get; set; } = 0;
    public float XFrequency { get; set; } = 0.0f;
    public float YFrequency { get; set; } = 0.0f;
    public float XAmplification { get; set; } = 0.0f;
    public float YAmplification { get; set; } = 0.0f;
    public byte ZLayer { get; set; } = 0;
	*/
    #endregion


    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
	{
		Sprite = GetNode<Sprite2D>("WobbleOrigin/DragOrigin/Sprite");
		OriginSprite = GetNode<Sprite2D>("WobbleOrigin/DragOrigin/Sprite/Origin");
		GrabArea = GetNode<Area2D>("WobbleOrigin/DragOrigin/Grab");
		DragOrigin = GetNode<Node2D>("WobbleOrigin/DragOrigin");
		Dragger = GetNode<Node2D>("WobbleOrigin/Dragger");
		WobbleOrigin = GetNode<Node2D>("WobbleOrigin");
		Offset = new Vector2(1.0f , 1.0f);

		Image image = new Image();
		Error imageLoadError = image.Load(LoadedSprite.Path);
		if(imageLoadError != Error.Ok) {
			if (LoadedImageData == null) {
				Global.ErrorHandler(imageLoadError);
				this.Dispose();
				return;
			}
			else {
				byte[] data = Marshalls.Base64ToRaw(LoadedImageData);
				Error bufferLoadError = image.LoadPngFromBuffer(data);
				if ( bufferLoadError != Error.Ok) {
					Global.ErrorHandler(bufferLoadError);
					this.QueueFree();
					return;
				}
			}
		}
		ImageTexture texture = ImageTexture.CreateFromImage(image);
		Texture = texture;
		ImageData = image;
		ImageSize = image.GetSize();
		Sprite.Texture = Texture;
		Bitmap bitmap = new Bitmap();
		bitmap.CreateFromImageAlpha(ImageData);
		Godot.Collections.Array<Vector2[]> polygons = bitmap.OpaqueToPolygons(new Rect2I(Vector2I.Zero , bitmap.GetSize()) , 4.0f);
		bool hasPolygons = polygons.Count > 0;
		foreach ( Vector2[] polygon in polygons) {
			CollisionPolygon2D collider = new CollisionPolygon2D();
			collider.Polygon = polygon;
			GrabArea.AddChild(collider);
			Line2D outline = OutlineScene.Instantiate<Line2D>();
			outline.Points = polygon;
			outline.AddPoint(outline.Points[0]);
			GrabArea.AddChild(outline);
		}
		Size = ImageData.GetSize();
		GrabArea.Position = Size * -0.5f ;
		Sprite.Offset = Offset;
		GrabArea.Position = ( Size * -0.5f ) + Offset;
		ChangeFrames();
		SetZLayer();
		if(Frames > 1) {
			RemakePolygon();
		}
		if (!hasPolygons) {
			RemakePolygon();
		}
		AddToGroup(LoadedSprite.Identification.ToString());
		await ToSignal(GetTree().CreateTimer(0.1) , SceneTreeTimer.SignalName.Timeout);
		if ( LoadedSprite.ParentIdentification != 0 ) {
			Godot.Collections.Array<Node> nodes = GetTree().GetNodesInGroup(LoadedSprite.ParentIdentification.ToString());
			GetParent().RemoveChild(this);
			nodes[0].GetNode<Sprite2D>("WobbleOrigin/DragOrigin/Sprite").AddChild(this);
			ParentSprite = nodes[0] as Sprite2D;
			Owner = nodes[0].GetNode("WobbleOrigin/DragOrigin/Sprite");
		}
		SetClip(IsClipped);
		if (Global.Filtering) {
			Sprite.TextureFilter = TextureFilterEnum.Linear;
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		Tick += 1;
		if (Global.HeldSprite == this) {
			GrabArea.Visible = true;
			OriginSprite.Visible = true;
		}
		else {
			GrabArea.Visible = false;
			OriginSprite.Visible = false;
		}
		Vector2 draggerGlobalPos = Dragger.GlobalPosition;
		if (IgnoreBounce) {
			draggerGlobalPos.Y -= Global.Main.BounceChange;
		}
		Drag(delta);
		Wobble();
		float length = draggerGlobalPos.Y - Dragger.GlobalPosition.Y;
		RotationalDrag(length , delta);
		Stretch(length , delta);
		if(GrabDelay > 1) {
			GrabDelay -= 1;
		}
		TalkBlink();
		Animation();
	}
    public void ReplaceSprite(string newPath)
    {
        Image image = new Image();
        Error imageLoadError = image.Load(newPath);
        if ( imageLoadError != Error.Ok ) {
            Global.ErrorHandler(imageLoadError);
            return;
        }
        LoadedSprite.Path = newPath;
        ImageTexture texture = ImageTexture.CreateFromImage(image);
        Texture = texture;
        ImageData = image;
        Sprite.Texture = texture;
        Bitmap bitmap = new Bitmap();
        bitmap.CreateFromImageAlpha(ImageData);
        Godot.Collections.Array<Vector2[]> polygons = bitmap.OpaqueToPolygons(new Rect2I(Vector2I.Zero , bitmap.GetSize()));
        foreach ( Node areaChild in GrabArea.GetChildren() ) {
            areaChild.QueueFree();
        }
        bool hasPolygons = polygons.Count > 0;
        foreach ( Vector2[] polygon in polygons ) {
            CollisionPolygon2D collider = new CollisionPolygon2D();
            collider.Polygon = polygon;
            GrabArea.AddChild(collider);
            Line2D outline = OutlineScene.Instantiate<Line2D>();
            outline.Points = polygon;
            outline.AddPoint(outline.Points[0]);
            GrabArea.AddChild(outline);
        }
        Size = ImageData.GetSize();
        Sprite.Offset = Offset;
        GrabArea.Position = ( Size * -0.5f ) + Offset;
        if ( !hasPolygons ) {
            RemakePolygon();
        }
    }

    public void Animation()
	{
		float speed = Mathf.Max(AnimationSpeed , Engine.MaxFps * 6.0f); 
		if(AnimationSpeed > 0 && Frames > 1 && Global.AnimationTick % (int)(speed / (float)AnimationSpeed) == 0) {
			Sprite.Frame = Sprite.Frame == Frames - 1 ? 0 : Sprite.Frame + 1;
		}
		if (Frames > 1) {
			RemakePolygon();
		}
	}
	public void SetZLayer()
	{
		Sprite.ZIndex = LoadedSprite.ZLayer;
	}
	public void TalkBlink()
	{
		int editValue = Global.Main.EditMode ? 1 : 0;
		int speakValue = Global.IsSpeaking ? 10 : 0;
		int blinkValue = Global.IsBlinking ? 20 : 0;
		int value = ( LoadedSprite.ShowOnTalk + ( LoadedSprite.ShowOnBlink * 3 ) ) + blinkValue + speakValue;
		double faded = 0.2 * editValue;
		int containsInArr = new Godot.Collections.Array(){ 0 , 10 , 20 , 30 , 1 , 21 , 12 , 32 , 3 , 13 , 4 , 15 , 26 , 36 , 27 , 38 }.Contains(value) ? 1 : 0;
		Color newColor = new Color(Sprite.SelfModulate , (float)Mathf.Max(containsInArr , faded));
		Sprite.SelfModulate = newColor;
	}
	public void PhysicsProcess()
	{
		if (Global.HeldSprite == null ) {
			Vector2 direction = PressingDirection();
			if (Input.IsActionPressed("Origin")) {
				MoveOrigin(direction);
			}
			else {
				MoveSprite(direction);
			}
		}
		else {
			SetPhysicsProcess(false);
		}
	}
	public Vector2 PressingDirection()
	{
		return new Vector2(Input.GetActionStrength("MoveLeft") - Input.GetActionStrength("MoveRight") , Input.GetActionStrength("MoveUp") - Input.GetActionStrength("MoveDown"));
	}
	public void MoveSprite(Vector2 direction)
	{
		HeldTicks = direction != Vector2.Zero ? HeldTicks + 1 : 0;
		if(HeldTicks > 30 || HeldTicks == 1) {
			int multipliter = HeldTicks == 1 ? 1 : 2;
			Position -= direction * multipliter;
		}
		Position = new Vector2 (Position.X, Position.Y);
	}	
	public void MoveOrigin(Vector2 direction)
	{
		OriginTick = direction != Vector2.Zero ? OriginTick + 1 : 0;
		if( OriginTick > 30 || OriginTick == 1) {
			int multipliter = OriginTick == 1 ? 1 : 2;
			Offset += direction * multipliter;
			Position -= direction * multipliter;
		}
		Offset = new Vector2 (Offset.X, Offset.Y);
		Sprite.Offset = Offset;
		GrabArea.Position = ( Size * -0.5f ) + Offset;
	}
	public void Drag(double delta)
	{
		if( LoadedSprite.DragSpeed == 0) {
			Dragger.GlobalPosition = WobbleOrigin.GlobalPosition;
		}
		else {
			Dragger.GlobalPosition = Dragger.GlobalPosition.Lerp(WobbleOrigin.GlobalPosition , (float)(( delta * 20 ) / LoadedSprite.DragSpeed ));
			DragOrigin.GlobalPosition = Dragger.GlobalPosition;
		}
	}
	public void Wobble()
	{
		Vector2 wavePosition = new Vector2(MathF.Sin(Tick * LoadedSprite.XFrequency) * LoadedSprite.XAmplification , MathF.Sin(Tick * LoadedSprite.YFrequency) * LoadedSprite.YAmplification);
		WobbleOrigin.Position = wavePosition;
	}
	public void RotationalDrag(float length,double delta)
	{
		float yVelocity = Mathf.Clamp(length * LoadedSprite.RotationalDragStrength , RotationaLimitMin , RotationaLimitMax);
		Sprite.Rotation = (float) Mathf.LerpAngle((double)Sprite.Rotation , (double)Mathf.DegToRad(yVelocity) , 0.25);
	}
	public void Stretch(float length,double delta)
	{
		float yVelocity = ( length * StretchAmount * 0.1f );
		Vector2 target = new Vector2(1.0f -  yVelocity , 1.0f + yVelocity );
		Sprite.Scale = Sprite.Scale.Lerp(target , 0.5f);
	}
	public void ChangeCollision(bool toggle)
	{
		GrabArea.Monitorable = toggle;
	}
	public void ChangeFrames()
	{
		Sprite.Hframes = Frames;
		Sprite.Frame = 0;
	}
	public void RemakePolygon()
	{
		if (RemadePolygon) {
			return;
		}
		foreach (Node child in GrabArea.GetChildren()) {
			child.QueueFree();
		}
		CollisionShape2D collider = new CollisionShape2D();
		RectangleShape2D shape = new RectangleShape2D();
		shape.Size = new Vector2(ImageSize.Y,ImageSize.Y);
		collider.Shape = shape;
		collider.Position = new Vector2(ImageSize.X,ImageSize.Y) + new Vector2(0.5f,0.5f);
		GrabArea.AddChild(collider);
		float vecPoint = ImageSize.Y * 0.5f;
		Line2D outline = OutlineScene.Instantiate<Line2D>();
		outline.AddPoint(new Vector2(-vecPoint , -vecPoint));
		outline.AddPoint(new Vector2(vecPoint , -vecPoint));
		outline.AddPoint(new Vector2(vecPoint , vecPoint));
		outline.AddPoint(new Vector2(-vecPoint , vecPoint));
		outline.AddPoint(new Vector2(-vecPoint , -vecPoint));
		outline.Position = collider.Position;
		GrabArea.AddChild (outline);
		RemadePolygon = true;
	}
	public void SetClip(bool toggle)
	{
		if(toggle) {
			Sprite.ClipChildren = ClipChildrenMode.AndDraw;
			foreach (SpriteObject node in GetAllLinkedSprites()) {
				node.ZIndex = LoadedSprite.ZLayer;
				node.SetZLayer();
			}
		}
		else {
			Sprite.ClipChildren = ClipChildrenMode.Disabled;
		}
		IsClipped = toggle;
	}
	public Godot.Collections.Array<SpriteObject> GetAllLinkedSprites()
	{
		Godot.Collections.Array<Node> nodes = GetTree().GetNodesInGroup("Saved");
		Godot.Collections.Array<SpriteObject> linkedSprites = new Godot.Collections.Array<SpriteObject>();
		foreach(SpriteObject node in nodes) {
			if (node.LoadedSprite.ParentIdentification == LoadedSprite.Identification ) {
				linkedSprites.Add(node);
			}
		}
		return linkedSprites;
    }
}
