// TRANSLATED

using Godot;
using static GlobalClass;

public partial class UserMouseCursor : Node2D
{
	public string Text { get; set; } = string.Empty;
	public Label Label { get; set; } = null;
	public Area2D Area { get; set; } = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.Mouse = this;
		Label = GetNode<Label>("Tooltip/Label");
		Area  = GetNode<Area2D>("Area2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Global.Main.EditMode) {
			if ( Text != string.Empty) {
				Label.Text = Text;
				Visible = true;
			}
			else {
				Visible = false;
			}
			GlobalPosition = GetGlobalMousePosition();
			if (Input.IsActionJustPressed("MouseLeft")) {
				Global.Select(Area.GetOverlappingAreas());
			}
		}
		else {
			Visible = false;
		}
		Text = string.Empty;
	}
}
