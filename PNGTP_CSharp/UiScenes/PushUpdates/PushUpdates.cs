using Godot;
using static GlobalClass;
public partial class PushUpdates : Node2D
{
    public VBoxContainer VBox { get; set; } = null;
    public uint Tick { get; private set; } = 0;

    public override void _Ready()
    {
        base._Ready();
        Global.UpdatePusherNode = this;
        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Tick++;
        if ( Tick > 239 ) {
            Modulate = new Color(Modulate , Modulate.A - (float) delta);
            if ( Modulate.A <= 0.0 ) {
                foreach ( Node Child in VBox.GetChildren() ) {
                    Child.QueueFree();
                }
                SetProcess(false);
            }
        }
    }

    public void PushUpdate(string message)
    {
        Label label = new Label();
        label.Text = message;
        label.AddThemeColorOverride("FontOutlineColor" , AppColors.BLACK);
        label.AddThemeConstantOverride("OudlineSize" , 6);
        VBox.AddChild(label);

        int childCount = VBox.GetChildCount();
        if ( childCount > 5 ) {
            VBox.GetChild(0).QueueFree();
        }
        Color modulateColor = new Color(Modulate , 1.0f);
        Modulate = modulateColor;
        Tick = 0;
        SetProcess(true);
    }
}
