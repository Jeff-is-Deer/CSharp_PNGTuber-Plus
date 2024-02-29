using Godot;
using Godot.Collections;
using Newtonsoft.Json;
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
        public Dictionary<byte , json_AvatarPart> Parts { get; set; }
        public bool BounceOnCostumeChange { get; set; }
        public int BounceStrength { get; set; }
        public int Gravity { get; set; }
        public int BlinkChance { get; set; }
        public float BlinkSpeed { get; set; }
    }

    public string SettingsFile { get; } = "user://Settings.jefftube";
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
        if (!FileAccess.FileExists(SettingsFile) ) {
            EmitSignal(SignalName.UserSettingsFailed);
        }
        LoadSettings();
    }
    public void Backup()
    {
        string formattedDate = DateTime.Now.ToString("MMddyyyy_HHmmss");
        BackupSettings(formattedDate);
        BackupAvatar(formattedDate);
    }
    private void BackupSettings(string formattedDate)
    {
        string originalSettingsContents;
        string backupName = $"Settings_{formattedDate}.jefftube";
        string backupFile = $"{BackupPath}/{backupName}";
        using ( FileAccess originalSave = FileAccess.Open(SettingsFile , FileAccess.ModeFlags.Read) ) {
            originalSettingsContents = OS.GetName() == "Windows" ? originalSave.GetAsText() : originalSave.GetAsText(true);
        }
        using ( FileAccess saveBackup = FileAccess.Open(backupFile , FileAccess.ModeFlags.Write) ) {
            saveBackup.StoreString(originalSettingsContents);
        }
    }
    private void BackupAvatar(string formattedDate)
    {
        string originalSaveContents;
        string backupName = $"MySpecialAvatar_{formattedDate}.jefftube";
        string backupFile = $"{BackupPath}/{backupName}";
        using ( FileAccess originalSave = FileAccess.Open(SavedAvatarFile , FileAccess.ModeFlags.Read) ) {
            originalSaveContents = OS.GetName() == "Windows" ? originalSave.GetAsText() : originalSave.GetAsText(true);
        }
        using ( FileAccess saveBackup = FileAccess.Open(backupFile , FileAccess.ModeFlags.Write) ) {
            saveBackup.StoreString(originalSaveContents);
        }
    }
    private void LoadSettings()
    {
        ProgramSettings savedSettings;
        using ( FileAccess settingsFile = FileAccess.Open(SettingsFile , FileAccess.ModeFlags.Read) ) {
            savedSettings = JsonConvert.DeserializeObject<ProgramSettings>(settingsFile.GetAsText());
        }
        Global.GetWindow().Size = new Vector2I(savedSettings.WindowSize[0] , savedSettings.WindowSize[1]);
        Global.BackgroundColor = new Color(savedSettings.BackgroundColor);
        Global.CurrentAvatarFile = savedSettings.DefaultAvatarFile;
        Engine.MaxFps = savedSettings.MaxFps;
        Global.Main.MicrophoneVolumeSlider.Value = savedSettings.MicrophoneVolume;
        Global.Main.MicrophoneSensitivitySlider.Value = savedSettings.ActivationSensitivity;
    }
    public void SaveSettings()
    {
        Backup();
        ProgramSettings toSave = GetCurrentProgramSettings();
        string json = JsonConvert.SerializeObject(toSave);
        using ( FileAccess newSave = FileAccess.Open(SavedAvatarFile , FileAccess.ModeFlags.Write) ) {
            newSave.StoreString(json);
        }
    }
    public Avatar LoadAvatar()
    {
        Avatar result;
        using ( FileAccess file = FileAccess.Open(SavedAvatarFile , FileAccess.ModeFlags.Read) ) {
            string json = OS.GetName() == "Windows" ? file.GetAsText() : file.GetAsText(true);
            result = JsonConvert.DeserializeObject<Avatar>(json);
        }
        return result;
    }
    public void SaveAvatar(Avatar avatar)
    {
        Backup();
        AvatarSettings toSave = new AvatarSettings() {
            Parts = ConvertAvatarPartsToSerializable(avatar) ,
            BounceOnCostumeChange = avatar.BounceOnCostumeChange ,
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
        ProgramSettings result = new ProgramSettings() {
            WindowSize = new int[2] { Global.GetWindow().Size.X , Global.GetWindow().Size.Y } ,
            BackgroundColor = Global.BackgroundColor.ToHtml() ,
            MaxFps = Engine.MaxFps ,
            DefaultAvatarFile = Global.CurrentAvatarFile ,
            MicrophoneVolume = Global.Main.MicrophoneVolumeSlider.Value ,
            ActivationSensitivity = Global.Main.MicrophoneSensitivitySlider.Value
        };
        return result;
    }
    private Dictionary<byte,json_AvatarPart> ConvertAvatarPartsToSerializable(Avatar avatar)
    {
        Dictionary<byte , json_AvatarPart> result = new Dictionary<byte , json_AvatarPart>();
        foreach(byte partId in avatar.Parts.Keys) {
            json_AvatarPart toAdd = new json_AvatarPart() {
                ID = partId ,
                PID = avatar.Parts[partId].SpriteData.PID ,
                Children = avatar.Parts[partId].SpriteData.Children,
                FilePath = avatar.Parts[partId].SpriteData.FilePath,
                Type = avatar.Parts[partId].SpriteData.Type,
                Base64ImageData = avatar.Parts[partId].SpriteData.Base64ImageData,
                ApdOffset = new float[2] { avatar.Parts[partId].SpriteData.Offset.X, avatar.Parts[partId].SpriteData.Offset.Y },
                ApdPosition = new float[2] { avatar.Parts[partId].SpriteData.Offset.X, avatar.Parts[partId].SpriteData.Offset.Y },
                AnimationSpeed = avatar.Parts[partId].SpriteData.AnimationSpeed,
                IsClipped = avatar.Parts[partId].SpriteData.IsClipped,
                IgnoresBounce = avatar.Parts[partId].SpriteData.IgnoresBounce,
                VisibleOnCostumeLayer = avatar.Parts[partId].SpriteData.VisibleOnCostumeLayer,
                NumberOfFrames = avatar.Parts[partId].SpriteData.NumberOfFrames,
                RotationalLimitMaximum = avatar.Parts[partId].SpriteData.RotationalLimitMaximum,
                RotationalLimitMinimum = avatar.Parts[partId].SpriteData.RotationalLimitMinimum,
                DragSpeed = avatar.Parts[partId].SpriteData.DragSpeed,
                RotationalDragStrength = avatar.Parts[partId].SpriteData.RotationalDragStrength,
                StretchAmount = avatar.Parts[partId].SpriteData.StretchAmount,
                XAmplitude = avatar.Parts[partId].SpriteData.XAmplitude,
                YAmplitude = avatar.Parts[partId].SpriteData.YAmplitude,
                XFrequency = avatar.Parts[partId].SpriteData.XFrequency,
                YFrequency = avatar.Parts[partId].SpriteData.YFrequency,
                ShowOnBlink = avatar.Parts[partId].SpriteData.ShowOnBlink,
                ShowOnTalk = avatar.Parts[partId].SpriteData.ShowOnTalk,
                ZLayer = (byte)avatar.Parts[partId].SpriteData.ZIndex,
            };
            result.Add(partId , toAdd);
        }
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
        using ( FileAccess newSettings = FileAccess.Open(SettingsFile , FileAccess.ModeFlags.Write) ) {
            newSettings.StoreString(defaultSettings);
        }
    }
    private void event_BackupDirectoryDoesntExist()
    {
        DirAccess.MakeDirAbsolute(BackupPath);
    }

}
