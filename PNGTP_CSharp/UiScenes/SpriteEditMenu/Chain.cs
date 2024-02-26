using System;
using Godot;
using static GlobalClass;

public partial class Chain : Node2D
{
    public Line2D Line { get; set; } = null;
    public Sprite2D Plug { get; set; } = null; 

    public override void _Ready()
    {
        base._Ready();
        Global.Chain = this;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        RenderPoints();
    }

    public void RenderPoints()
    {
        if(Global.SelectedSprite != null) {
            GlobalPosition = Global.SelectedSprite.GlobalPosition;
        }
        Line.ClearPoints();
        Line.AddPoint(Vector2.Zero);
        Line.AddPoint(ToLocal(GetGlobalMousePosition()));
        Plug.Position = GetLocalMousePosition();
        Plug.LookAt(GlobalPosition);
        Plug.RotationDegrees += 180;
    }

    public void Enable(bool enabled)
    {
        RenderPoints();
        SetProcess(enabled);
        Visible = enabled;
    }
}
