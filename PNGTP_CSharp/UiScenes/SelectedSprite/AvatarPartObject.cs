// TRANSLATED

using Godot;
using static GlobalClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

public partial class AvatarPartObject : Node2D
{
	private const byte SPRITE = 1;
	private const byte LOAD_FROM_IMAGE = 2;
	private const byte LOAD_FROM_BASE64 = 3;
	public class ImageData
	{
		public Image image { get; set; } = null;
		public ImageTexture texture { get; set; } = null;
		public Error error { get; set; } = Error.PrinterOnFire;
		public bool freeHim { get; set; } = false;
		public Vector2 size { get; set; } = Vector2.Zero;
		public Bitmap bitmap { get; set; } = null;
		public CollisionPolygon2D collisionPolygon { get; set; } = null;
	}
	// Scene nodes
	public PackedScene OutlineScene { get; set; } = ResourceLoader.Load<PackedScene>("res://UiScenes/SelectedSprite/Outline.tscn");
	public Sprite2D OriginSprite { get; set; } = null;
	public Area2D GrabArea { get; set; } = null;
	public Node2D DragOrigin { get; set; } = null;
	public Node2D Dragger { get; set; } = null;
	public Node2D PivotPoint { get; set; } = null;

	// Sprite data
	public AvatarPartSprite PartData { get; set; } = null;
	public ImageData LoadedImageData { get; set; } = null;

	// Visuals
	public Vector2 MouseOffset { get; set; } = Vector2.Zero;
	public int GrabDelay { get; set; } = 0;

	// Movement
	public int HeldTicks { get; set; } = 0;

	// Origin
	public int OriginTick { get; set; } = 0;
	public Vector2 OriginOffset { get; set; } = Vector2.Zero;


	// Animation
	public bool RemadePolygon { get; set; } = false;
	public int Tick { get; set; } = 0;


	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		PartData = GetNode<AvatarPartSprite>("PivotPoint/DragOrigin/AvatarPart");
		OriginSprite = GetNode<Sprite2D>("PivotPoint/DragOrigin/AvatarPart/Origin");
		GrabArea = GetNode<Area2D>("PivotPoint/DragOrigin/Grab");
		DragOrigin = GetNode<Node2D>("PivotPoint/DragOrigin");
		Dragger = GetNode<Node2D>("PivotPoint/Dragger");
		PivotPoint = GetNode<Node2D>("PivotPoint");

		SetLoadedImageData(PartData);
		SetImageGrabArea();
		SetImagePositions();
		ChangeFrames();
		SetZLayer();
		RemakePolygon();
		AddToGroup(PartData.ID.ToString());
		await ToSignal(GetTree().CreateTimer(0.1) , SceneTreeTimer.SignalName.Timeout);
		SetClip(PartData.IsClipped);
		if ( Global.Filtering ) {
			PartData.TextureFilter = TextureFilterEnum.Linear;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Tick += 1;
		if ( Global.SelectedAvatarPart == this ) {
			GrabArea.Visible = true;
			OriginSprite.Visible = true;
		}
		else {
			GrabArea.Visible = false;
			OriginSprite.Visible = false;
		}
		Vector2 draggerGlobalPos = Dragger.GlobalPosition;
		if ( PartData.IgnoresBounce ) {
			draggerGlobalPos.Y -= Global.Main.BounceChange;
		}
		Drag(delta);
		Wobble();
		float length = draggerGlobalPos.Y - Dragger.GlobalPosition.Y;
		RotationalDrag(length , delta);
		Stretch(length , delta);
		if ( GrabDelay > 1 ) {
			GrabDelay -= 1;
		}
		TalkBlink();
		Animation();
	}
	private void SetLoadedImageData(AvatarPartSprite partData)
	{
		LoadedImageData = new ImageData();
        byte loadOption = partData.Base64ImageData == string.Empty ? LOAD_FROM_IMAGE : LOAD_FROM_BASE64;
        switch ( loadOption ) {
            case LOAD_FROM_IMAGE:
                LoadedImageData.error = LoadedImageData.image.Load(partData.FilePath);
                break;
            case LOAD_FROM_BASE64:
                LoadedImageData.error = LoadedImageData.image.LoadPngFromBuffer(Marshalls.Base64ToRaw(partData.Base64ImageData));
                break;
        }
        if ( LoadedImageData.error != Error.Ok ) {
			QueueFree();
			return;
        }
        PartData.Texture = ImageTexture.CreateFromImage(LoadedImageData.image);
        LoadedImageData.size = LoadedImageData.image.GetSize();
        LoadedImageData.bitmap = new Bitmap();
        LoadedImageData.bitmap.CreateFromImageAlpha(LoadedImageData.image);
    }
	private void SetImageGrabArea()
	{
        Godot.Collections.Array<Vector2[]> polygons = LoadedImageData.bitmap.OpaqueToPolygons(new Rect2I(Vector2I.Zero , LoadedImageData.bitmap.GetSize()) , 4.0f);
        foreach ( Vector2[] polygon in polygons ) {
            LoadedImageData.collisionPolygon = new CollisionPolygon2D();
			Line2D outline = OutlineScene.Instantiate<Line2D>();

            LoadedImageData.collisionPolygon.Polygon = polygon;
            GrabArea.AddChild(LoadedImageData.collisionPolygon);
			outline.Points = polygon;
			outline.AddPoint(outline.Points[0]);
			GrabArea.AddChild(outline);
        }
    }
	private void SetImagePositions()
	{
		PartData.Offset = OriginOffset;
		GrabArea.Position = (LoadedImageData.size * -0.5f) + OriginOffset;
    }
	private void SetImageParent()
	{
        if ( PartData.PID != 0 ) {
			List<AvatarPartObject> nodes = GetTree().GetNodesInGroup(PartData.PID.ToString()).OfType<AvatarPartObject>().ToList(); ;
            GetParent().RemoveChild(this);
            nodes[0].GetNode<AvatarPartObject>("PivotPoint/DragOrigin/AvatarPart").AddChild(this);
            PartData.ParentPart = nodes[0].PartData;
			nodes[0].AddChild(this);
            Owner = nodes[0].GetNode("PivotPoint/DragOrigin/AvatarPart");
        }
    }
    public void ReplaceSprite(string newPath)
	{
		Image image = new Image();
		Error imageLoadError = image.Load(newPath);
		if ( imageLoadError != Error.Ok ) {
			Global.ErrorHandler(imageLoadError);
			return;
		}
		PartData.FilePath = newPath;
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
		float speed = Mathf.Max(PartData.AnimationSpeed , Engine.MaxFps * 6.0f);
		if ( PartData.AnimationSpeed > 0 && PartData.Frames > 1 && Global.AnimationTick % ( int ) ( speed / ( float ) PartData.AnimationSpeed ) == 0 ) {
			Sprite.Frame = Sprite.Frame == PartData.Frames - 1 ? 0 : Sprite.Frame + 1;
		}
		if ( PartData.Frames > 1 ) {
			RemakePolygon();
		}
	}
	public void SetZLayer()
	{
		ZIndex = PartData.ZIndex;
	}
	public void TalkBlink()
	{
		int editValue = Global.Main.EditMode ? 1 : 0;
		int speakValue = Global.IsSpeaking ? 10 : 0;
		int blinkValue = Global.IsBlinking ? 20 : 0;
		int value = ( PartData.ShowOnTalk + ( PartData.ShowOnBlink * 3 ) ) + blinkValue + speakValue;
		double faded = 0.2 * editValue;
		int containsInArr = new Godot.Collections.Array() { 0 , 10 , 20 , 30 , 1 , 21 , 12 , 32 , 3 , 13 , 4 , 15 , 26 , 36 , 27 , 38 }.Contains(value) ? 1 : 0;
		Color newColor = new Color(Sprite.SelfModulate , ( float ) Mathf.Max(containsInArr , faded));
		Sprite.SelfModulate = newColor;
	}
	public void PhysicsProcess()
	{
		if ( Global.SelectedSprite == null ) {
			Vector2 direction = PressingDirection();
			if ( Input.IsActionPressed("Origin") ) {
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
		if ( HeldTicks > 30 || HeldTicks == 1 ) {
			int multipliter = HeldTicks == 1 ? 1 : 2;
			Position -= direction * multipliter;
		}
		Position = new Vector2(Position.X , Position.Y);
	}
	public void MoveOrigin(Vector2 direction)
	{
		OriginTick = direction != Vector2.Zero ? OriginTick + 1 : 0;
		if ( OriginTick > 30 || OriginTick == 1 ) {
			int multipliter = OriginTick == 1 ? 1 : 2;
			Offset += direction * multipliter;
			Position -= direction * multipliter;
		}
		Offset = new Vector2(Offset.X , Offset.Y);
		Sprite.Offset = Offset;
		GrabArea.Position = ( Size * -0.5f ) + Offset;
	}
	public void Drag(double delta)
	{
		if ( PartData.DragSpeed == 0 ) {
			Dragger.GlobalPosition = PivotPoint.GlobalPosition;
		}
		else {
			Dragger.GlobalPosition = Dragger.GlobalPosition.Lerp(PivotPoint.GlobalPosition , ( float ) ( ( delta * 20 ) / PartData.DragSpeed ));
			DragOrigin.GlobalPosition = Dragger.GlobalPosition;
		}
	}
	public void Wobble()
	{
		Vector2 wavePosition = new Vector2(MathF.Sin(Tick * PartData.XFrequency) * PartData.XAmplification , MathF.Sin(Tick * PartData.YFrequency) * PartData.YAmplification);
		PivotPoint.Position = wavePosition;
	}
	public void RotationalDrag(float length , double delta)
	{
		float yVelocity = Mathf.Clamp(length * PartData.RotationalDragStrength , PartData.RotationalLimitMinimum , PartData.RotationalLimitMaximum);
		Sprite.Rotation = ( float ) Mathf.LerpAngle(( double ) Sprite.Rotation , ( double ) Mathf.DegToRad(yVelocity) , 0.25);
	}
	public void Stretch(float length , double delta)
	{
		float yVelocity = ( length * PartData.SquishAmount * 0.1f );
		Vector2 target = new Vector2(1.0f - yVelocity , 1.0f + yVelocity);
		Sprite.Scale = Sprite.Scale.Lerp(target , 0.5f);
	}
	public void ChangeCollision(bool toggle)
	{
		GrabArea.Monitorable = toggle;
	}
	public void ChangeFrames()
	{
		PartData.Hframes = PartData.NumberOfFrames;
		PartData.NumberOfFrames = 0;
	}
	public void RemakePolygon()
	{
        if ( PartData.NumberOfFrames < 2 || LoadedImageData.bitmap.OpaqueToPolygons(new Rect2I(Vector2I.Zero , LoadedImageData.bitmap.GetSize()) , 4.0f).Count > 0 || RemadePolygon ) {
			return;
		}
		foreach ( Node child in GrabArea.GetChildren() ) {
			child.QueueFree();
		}
		CollisionShape2D collider = new CollisionShape2D();
		RectangleShape2D shape = new RectangleShape2D();
		shape.Size = new Vector2(LoadedImageData.size.Y , LoadedImageData.size.Y);
		collider.Shape = shape;
		collider.Position = new Vector2(LoadedImageData.size.Y , LoadedImageData.size.Y) + new Vector2(0.5f , 0.5f);
		GrabArea.AddChild(collider);
		float vecPoint = LoadedImageData.size.Y * 0.5f;
		Line2D outline = OutlineScene.Instantiate<Line2D>();
		outline.AddPoint(new Vector2(-vecPoint , -vecPoint));
		outline.AddPoint(new Vector2(vecPoint , -vecPoint));
		outline.AddPoint(new Vector2(vecPoint , vecPoint));
		outline.AddPoint(new Vector2(-vecPoint , vecPoint));
		outline.AddPoint(new Vector2(-vecPoint , -vecPoint));
		outline.Position = collider.Position;
		GrabArea.AddChild(outline);
		RemadePolygon = true;
	}
	public void SetClip(bool toggle)
	{
		if ( toggle ) {
			PartData.ClipChildren = ClipChildrenMode.AndDraw;
			foreach ( AvatarPartObject node in GetAllLinkedSprites() ) {
				node.ZIndex = PartData.ZIndex;
				node.SetZLayer();
			}
		}
		else {
			Sprite.ClipChildren = ClipChildrenMode.Disabled;
		}
		PartData.IsClipped = toggle;
	}
	public Godot.Collections.Array<AvatarPartObject> GetAllLinkedSprites()
	{
		Godot.Collections.Array<Node> nodes = GetTree().GetNodesInGroup("Saved");
		Godot.Collections.Array<AvatarPartObject> linkedSprites = new Godot.Collections.Array<AvatarPartObject>();
		foreach ( AvatarPartObject node in nodes ) {
			if ( node.PartData.PID == PartData.ID ) {
				linkedSprites.Add(node);
			}
		}
		return linkedSprites;
	}
}