using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using static GlobalClass;

public partial class FileHandling : Node
{
    [Signal]
    public delegate void ImageLoadFailedEventHandler(string error);
    [Signal]
    public delegate void LoadFailedEventHandler();
    [Signal]
    public delegate void SaveFailedEventHandler();
    [Signal]
    public delegate void UserSettingsFailedEventHandler();
    [Signal]
    public delegate void BackupDirectoryDoesntExistEventHandler();

    private class ProgramSettings
    {
        public int[] WindowSize { get; set; }
        public string BackgroundColor { get; set; }
        public string DefaultAvatarFile { get; set; }
        public int MaxFps { get; set; }
        public double MicrophoneVolume { get; set; }
        public double ActivationSensitivity { get; set; }
    }

    private class AvatarSettings
    {
        public Dictionary<byte , AvatarPart> Parts { get; set; }
        public bool IsBlinking { get; set; }
        public bool BounceOnCostumeChange { get; set; }
        public int BounceStrength { get; set; }
        public int Gravity { get; set; }
        public int BlinkChance { get; set; }
        public float BlinkSpeed { get; set; }
    }

    public string SettingsPath { get; } = "user://Settings.jefftube";
    public string BackupPath { get; } = "user://backups";
	public string SavedAvatarFile { get; set; }
    public override void _Ready()
    {
        ImageLoadFailed += event_ImageLoadFailed;
        LoadFailed += event_AvatarLoadFailed;
        SaveFailed += event_AvatarSaveFailed;
        UserSettingsFailed += event_UserSettingsFailed;
        BackupDirectoryDoesntExist += event_BackupDirectoryDoesntExist;
        if (DirAccess.Open(BackupPath) == null) {
            EmitSignal(SignalName.BackupDirectoryDoesntExist);
        }
        if (!FileAccess.FileExists(SettingsPath)) {
            EmitSignal(SignalName.UserSettingsFailed);
        }
        LoadSettings();
        Avatar.Parts = LoadAvatar();

    }
    public void Backup()
    {
        string originalSaveContents;
        string backupName = $"backup_{DateTime.Now:MMddyyyy_HHmmss}.jefftube";
        string backupFile = $"{BackupPath}/{backupName}";
        using ( FileAccess originalSave = FileAccess.Open(SavedAvatarFile , FileAccess.ModeFlags.Read) ) {
            originalSaveContents = OS.GetName() == "Windows" ? originalSave.GetAsText() : originalSave.GetAsText(true);
        }
        using ( FileAccess saveBackup = FileAccess.Open(BackupPath , FileAccess.ModeFlags.Write) ) {
            saveBackup.StoreString(originalSaveContents);
        }
    }
    private void LoadSettings()
    {
        ProgramSettings savedSettings;
        using ( FileAccess settingsFile = FileAccess.Open(SettingsPath , FileAccess.ModeFlags.Read) ) {
            savedSettings = JsonConvert.DeserializeObject<ProgramSettings>(settingsFile.GetAsText());
        }
        GetWindow().Size = new Vector2I(savedSettings.WindowSize[0] , savedSettings.WindowSize[1]);
        Global.BackgroundColor = new Color(savedSettings.BackgroundColor);
        SavedAvatarFile = savedSettings.DefaultAvatarFile;
        Engine.MaxFps = savedSettings.MaxFps;
        Global.Main.MicrophoneVolumeSlider.Value = savedSettings.MicrophoneVolume;
        Global.Main.MicrophoneSensitivitySlider.Value = savedSettings.ActivationSensitivity;
    }
    public void SaveSettings()
    {
        ProgramSettings toSave = GetCurrentProgramSettings();
        string json = JsonConvert.SerializeObject(Avatar.Parts);
        using ( FileAccess newSave = FileAccess.Open(SavedAvatarFile , FileAccess.ModeFlags.Write) ) {
            newSave.StoreString(json);
        }
    }
    public Dictionary<byte , AvatarPart> LoadAvatar()
    {
        Backup();
        Dictionary<byte , AvatarPart> result;
        using ( FileAccess file = FileAccess.Open(SavedAvatarFile , FileAccess.ModeFlags.Read) ) {
            string json = OS.GetName() == "Windows" ? file.GetAsText() : file.GetAsText(true);
            result = JsonConvert.DeserializeObject<Dictionary<byte , AvatarPart>>(json);
        }
        return result;
    }
    public void SaveAvatar(Avatar avatar)
    {
        AvatarSettings toSave = new AvatarSettings() {
            Parts = Avatar.Parts ,
            BounceOnCostumeChange = avatar.BounceOnCostumeChange ,
            IsBlinking = false ,
            BlinkChance = avatar.BlinkChance ,
            BlinkSpeed = avatar.BlinkSpeed ,
            BounceStrength = avatar.BounceStrength ,
            Gravity = avatar.Gravity
        };
        string json = JsonConvert.SerializeObject(toSave);
        using ( FileAccess newSave = FileAccess.Open(SavedAvatarFile , FileAccess.ModeFlags.Write) ) {
            newSave.StoreString(json);
        }
    }
    private ProgramSettings GetCurrentProgramSettings()
    {
        ProgramSettings result = new ProgramSettings();
        result.WindowSize = new int[2] { GetWindow().Size.X, GetWindow().Size.Y };
        result.BackgroundColor = Global.BackgroundColor.ToHtml();
        result.MaxFps = Engine.MaxFps;
        result.DefaultAvatarFile = SavedAvatarFile;
        result.MicrophoneVolume = Global.Main.MicrophoneVolumeSlider.Value;
        result.ActivationSensitivity = Global.Main.MicrophoneSensitivitySlider.Value;
        return result;
    }
    private void event_ImageLoadFailed(string error)
    {
        if ( Enum.TryParse(error , out Error result) ) {
            Global.ErrorHandler(result);
        }
        else {
            Global.ErrorHandler(Error.PrinterOnFire);
        }
    }
    private void event_AvatarSaveFailed()
    {

    }
    private void event_AvatarLoadFailed()
    {

    }
    private void event_UserSettingsFailed()
    {   
        string defaultSettings = "{\"WindowSize\":[1280,720],\"BackgroundColor\":\"#b8b8b8ff\",\"DefaultAvatarFile\":\"user://DefaultAvatar.jefftube\",\"MaxFps\":30,\"MicrophoneVolume\":  0.185,\"ActivationSensitivity\":  0.25}";
        using ( FileAccess newSettings = FileAccess.Open(SettingsPath , FileAccess.ModeFlags.Write) ) {
            newSettings.StoreString(defaultSettings);
        }
    }
    private void event_BackupDirectoryDoesntExist()
    {
        DirAccess.MakeDirAbsolute(BackupPath);
    }

}
