using Godot;
using System;

public partial class Saving : Node
{
	public string Key { get; } = "creature";
	public string SettingsPath { get; } = "user://settings.pngtp";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AvatarData? data = ReadSave(SettingsPath);
		if (data == null) {
			return;
		}
		else {
			
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public AvatarData ReadSave(string path)
	{
		if (path == "default") {
			return AvatarData.Default;
		}
		if(OS.HasFeature("web") {
			Variant JsonString = JavaScriptBridge.Eval($"window.localStorage.getItem(\"{Key}\");");
			if(JsonString.AsBool()) {
				return JsonString.As<AvatarData>();
			}
		}
	}
}
