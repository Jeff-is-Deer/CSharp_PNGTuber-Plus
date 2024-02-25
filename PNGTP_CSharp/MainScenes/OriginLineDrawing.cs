using Godot;
using System;

public partial class OriginLineDrawing : Node2D
{
	Line2D Horizontal { get; set; } = null;
	Line2D Vertical { get; set; } = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Horizontal = GetNode<Line2D>("Horizontal");
		Vertical = GetNode<Line2D>("Vertical");
	}
	public void DrawLine()
	{
		Vector2 size = GetViewport().GetVisibleRect().Size;
		Vertical.ClearPoints();
		Vertical.AddPoint(new Vector2(0.0f , -size.Y));
		Vertical.AddPoint(new Vector2(0.0f, size.Y));
		Horizontal.ClearPoints();
		Horizontal.AddPoint(new Vector2(-size.X , 0.0f));
		Horizontal.AddPoint(new Vector2(size.X , 0.0f));
	}
}
