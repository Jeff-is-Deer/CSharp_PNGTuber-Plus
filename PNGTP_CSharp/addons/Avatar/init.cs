#if TOOLS
using Godot;

[Tool]
public partial class AvatarPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        Script data = GD.Load<Script>("res://addons/AvatarPiece/Avatar.cs");
        Texture2D icon = GD.Load<Texture2D>("res://addons/AvatarPiece/SCHNOZER150.png");
        AddCustomType("Avatar" , "Sprite2D" , data , icon);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveCustomType("Avatar");
    }
}
#endif
