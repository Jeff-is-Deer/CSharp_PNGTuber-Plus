using Godot;
using static GlobalClass;

public partial class MicrophoneSelectButton : Control
{
	public string MicName { get; set; } = string.Empty;
	public Label Label { get; set; } = null;

    public override void _Ready()
    {
		Label = GetNode<Label>("Label");
		Label.Text = MicName;
    }

    // Called when the node enters the scene tree for the first time.
    public async void OnButtonPressed()
	{
		if ( !Visible ) {
			return;
		}
		AudioServer.InputDevice = MicName;
		Global.DeleteAllMicrophones();
		Global.CurrentMicrophone.Dispose();

		Visible = false;
		await ToSignal(GetTree().CreateTimer(1.0) , SceneTreeTimer.SignalName.Timeout);
		Global.CreateMicrophone();
	}
}
