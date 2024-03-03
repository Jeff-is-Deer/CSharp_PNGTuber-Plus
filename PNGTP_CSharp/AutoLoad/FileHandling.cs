using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using System;
using static GlobalClass;

public partial class FileHandling : Node
{
    [Signal]
    public delegate void ImageLoadFailedEventHandler(long errorValue);
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
                PID = avatar.Parts[partId].PartData.PID ,
                Children = avatar.Parts[partId].PartData.Children,
                FilePath = avatar.Parts[partId].PartData.FilePath,
                Type = avatar.Parts[partId].PartData.Type,
                Base64ImageData = avatar.Parts[partId].PartData.Base64ImageData,
                ApdOffset = new float[2] { avatar.Parts[partId].PartData.Offset.X, avatar.Parts[partId].PartData.Offset.Y },
                ApdPosition = new float[2] { avatar.Parts[partId].PartData.Offset.X, avatar.Parts[partId].PartData.Offset.Y },
                AnimationSpeed = avatar.Parts[partId].PartData.AnimationSpeed,
                IsClipped = avatar.Parts[partId].PartData.IsClipped,
                IgnoresBounce = avatar.Parts[partId].PartData.IgnoresBounce,
                VisibleOnCostumeLayer = avatar.Parts[partId].PartData.VisibleOnCostumeLayer,
                NumberOfFrames = avatar.Parts[partId].PartData.NumberOfFrames,
                RotationalLimitMaximum = avatar.Parts[partId].PartData.RotationalLimitMaximum,
                RotationalLimitMinimum = avatar.Parts[partId].PartData.RotationalLimitMinimum,
                DragSpeed = avatar.Parts[partId].PartData.DragSpeed,
                RotationalDragStrength = avatar.Parts[partId].PartData.RotationalDragStrength,
                SquishAmount = avatar.Parts[partId].PartData.SquishAmount,
                XAmplitude = avatar.Parts[partId].PartData.XAmplitude,
                YAmplitude = avatar.Parts[partId].PartData.YAmplitude,
                XFrequency = avatar.Parts[partId].PartData.XFrequency,
                YFrequency = avatar.Parts[partId].PartData.YFrequency,
                ShowOnBlink = avatar.Parts[partId].PartData.ShowOnBlink,
                ShowOnTalk = avatar.Parts[partId].PartData.ShowOnTalk,
                ZLayer = (byte)avatar.Parts[partId].PartData.ZIndex,
            };
            result.Add(partId , toAdd);
        }
        return result;
    }
    private void event_ImageLoadFailed(long error)
    {
        Global.ErrorHandler((Error)error);
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
