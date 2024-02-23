using Godot;
using Godot.Collections;
using System.Collections.Generic;
using static GlobalClass;

public partial class SpriteListViewer : Node2D
{
    public PackedScene SpriteListObject { get; } = ResourceLoader.Load<PackedScene>("res://UiScenes/SpriteList/SpriteListObject.tscn");
    public VBoxContainer Container { get; set; } = null;
    public SpriteData Sprite { get; set; } = null;

    public override void _Ready()
    {
        Container = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
        Global.SpriteList = this;
    }

    public async void UpdateData()
    {
        ClearContainer();
        await ToSignal(GetTree().CreateTimer(0.15),SceneTreeTimer.SignalName.Timeout);
        Array<Node> allSprites = GetTree().GetNodesInGroup("saved");
        List<SpriteListObject> childSprites = new List<SpriteListObject >();
        Array<SpriteListObject> parsedSprites = new Array<SpriteListObject>();
        foreach(SpriteObject sprite in allSprites) {
            SpriteListObject spriteListObject = SpriteListObject.Instantiate<SpriteListObject>();
            spriteListObject.SpritePath = sprite.LoadedSprite.Path;
            spriteListObject.Sprite = Sprite;
            spriteListObject.Sprite.ParentIdentification = Sprite.ParentIdentification;
            if(Sprite.Identification != 0) {
                childSprites.Add(spriteListObject);
            }
            parsedSprites.Add(spriteListObject);
            Container.AddChild(spriteListObject);
            foreach(SpriteListObject childSprite in childSprites) {
                byte index = 0;
                foreach (SpriteListObject parsedSprite in parsedSprites) {
                    /* I FUCKED UP THE CLASS ORGANIZATION GOD DAMMIT AAAAAAAAAAAA
                     * Sprites should have a property storing their parent sprite's data
                     * I will do this later when brain don't going bdriarhakjhsdrrrrrrrr
                    */
                }
            }
        }
    }
    public void ClearContainer()
    {
        foreach(Node child in Container.GetChildren()) {
            child.QueueFree();
        }
    }
    public void UpdateAllVisibility()
    {
        foreach (SpriteListObject child in Container.GetChildren()) {
            child.UpdateVisibility();
        }
    }
}


