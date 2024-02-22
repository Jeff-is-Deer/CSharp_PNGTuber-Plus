// TRANSLATED

using Godot;

public partial class MicInputSelect : Node2D
{
	public PackedScene ButtonScene { get; set; } = null;
	public VBoxContainer Container { get; set; } = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonScene = ResourceLoader.Load<PackedScene>("res://UiScenes/MicrophoneSelect/MicSelectButton.tscn");
		Container = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");
		ShowMicMenu();
	}

	public void ShowMicMenu()
	{
		foreach (Node child in Container.GetChildren()) {
			child.QueueFree();
		}
		string[] inputDeviceList = AudioServer.GetInputDeviceList();
		foreach (string device in inputDeviceList) {
			Button newButton = ButtonScene.Instantiate<Button>();
			newButton.Text = device;
			Container.AddChild(newButton);
		}
	}
}
