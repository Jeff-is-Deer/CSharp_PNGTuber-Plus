// TRANSLATED

using Godot;
using static GlobalClass;

public partial class EditControls : Node2D
{
	public Button AddButton { get; set; } = null;
	public Sprite2D AddSprite { get; set; } = null;
	public Button LinkButton { get; set; } = null;
	public Sprite2D LinkSprite { get; set; } = null;
	public Button ExitButton { get; set; } = null;
	public Sprite2D ExitSprite { get; set; } = null;
	public Button SaveButton { get; set; } = null;
	public Sprite2D SaveSprite { get; set; } = null;
	public Button LoadButton { get; set; } = null;
	public Sprite2D LoadSprite { get; set; } = null;
	public Button ReplicateButton { get; set; } = null;
	public Sprite2D ReplicateSprite { get; set; } = null;
	public Button DuplicateButton { get; set; } = null;
	public Sprite2D DuplicateSprite { get; set; } = null;
	public Godot.Collections.Array<Button> Buttons { get; set; } = null;
	public Godot.Collections.Array<Sprite2D> Sprites { get; set; } = null;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddButton = GetNode<Button>("Add/AddButton");
		AddSprite = GetNode<Sprite2D>("Add/AddSprite");
		LinkButton = GetNode<Button>("Link/LinkButton");
		LinkSprite = GetNode<Sprite2D>("Link/LinkSprite");
		ExitButton = GetNode<Button>("Exit/ExitButton");
		ExitSprite = GetNode<Sprite2D>("Exit/ExitSprite");
		SaveButton = GetNode<Button>("Save/SaveButton");
		SaveSprite = GetNode<Sprite2D>("Save/SaveSprite");
		LoadButton = GetNode<Button>("Load/LoadButton");
		LoadSprite = GetNode<Sprite2D>("Load/LoadSprite");
		ReplicateButton = GetNode<Button>("Replicate/ReplicateButton");
		ReplicateSprite = GetNode<Sprite2D>("Replicate/ReplicateSprite");
		DuplicateButton = GetNode<Button>("Duplicate/DuplicateButton");
		DuplicateSprite = GetNode<Sprite2D>("Duplicate/DuplicateSpite");

		Buttons = new Godot.Collections.Array<Button>() { AddButton, LinkButton, ExitButton, SaveButton, LoadButton, ReplicateButton, DuplicateButton };
		Sprites = new Godot.Collections.Array<Sprite2D>() { AddSprite, LinkSprite, ExitSprite, SaveSprite, LoadSprite, ReplicateSprite , DuplicateSprite };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int spriteIndex = 0;
		for (int buttonIndex = 0; buttonIndex < Buttons.Count; buttonIndex++) {
			if (Buttons[buttonIndex] == null) {
				continue;
			}
			Vector2 rect2X = Buttons[buttonIndex].GetParent<Sprite2D>().Position - new Vector2(24.0f , 24.0f);
			bool rectHasPoint = new Rect2(rect2X , Buttons[buttonIndex].Size).HasPoint(GetLocalMousePosition());
            if ( rectHasPoint ) {
				Sprites[spriteIndex].Scale = Sprites[spriteIndex].Scale.Lerp(new Vector2(1.2f , 1.2f) , 0.2f);
				if (Input.IsActionPressed("MouseLeft")) {
					Sprites[spriteIndex].Scale = new Vector2(0.6f , 0.6f);
				}
				switch(buttonIndex) {
					case 0:
						Global.Mouse.Text = "Add new sprite";
						break;
					case 1:
						Global.Mouse.Text = "Link sprite";
						break;
					case 2:
						Global.Mouse.Text = "Exit edit mode";
						break;
					case 3:
                        Global.Mouse.Text = "Save avatar";
                        break;
					case 4:
                        Global.Mouse.Text = "Load avatar";
                        break;
					case 5:
                        Global.Mouse.Text = "Replace sprite";
                        break;
					case 6:
                        Global.Mouse.Text = "Duplicate sprite";
                        break;
				}
            }
			else {
				Sprites[spriteIndex].Scale = Sprites[spriteIndex].Scale.Lerp(new Vector2(1.0f , 1.0f) , 0.2f);
            }
			spriteIndex++;
		}
		Color newColor = Global.HeldSprite == null ? new Color(0.184314f , 0.309804f , 0.309804f) : new Color(1,1,1);

		LinkSprite.GetParent<Sprite2D>().Modulate = newColor;
		ReplicateSprite.GetParent<Sprite2D>().Modulate = newColor;
		DuplicateSprite.GetParent<Sprite2D>().Modulate = newColor;
	}

	public void Notification(int i) 
	{
        if (i == 30) {
            float x = GetNode<Area2D>("MoveMenuDown").Position.X;
            float y = GetWindow().Size.Y;
            GetNode<Area2D>("MoveMenuDown").Position = new Vector2(x,y);
		}
	}
}
