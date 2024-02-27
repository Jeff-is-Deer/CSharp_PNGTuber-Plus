using System;
using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using static GlobalClass;

public partial class Avatar : Sprite2D
{
    [Signal] 
    public delegate void ImageLoadFailedEventHandler(string error);
    [Signal]
    public delegate void LoadFailedEventHandler();
    [Signal]
    public delegate void SaveFailedEventHandler();
    private string savePath { get; } = "user://MySpecialAvatar.jefftube";
    public Dictionary<byte , AvatarPart> Parts { get; set; } 

    public override void _Ready()
    {
        ImageLoadFailed += event_ImageLoadFailed;
        LoadFailed += event_AvatarLoadFailed;
        SaveFailed += event_AvatarSaveFailed;
        Parts = Load();
    }

#nullable enable
    public Image? GetImageFromPath(string filePath)
    {
        Image result = new Image();
        Error error = result.Load(filePath);
        if (error != Error.Ok) {
            string errorName = Enum.GetName(typeof(Error) , error);
            EmitSignal(SignalName.ImageLoadFailed , errorName);
            return null;
        }
        return result;
    }
    public Image? GetImageFromBuffer(string base64)
    {
        Image result = new Image();
        byte[] convertedData = Marshalls.Base64ToRaw(base64);
        Error error = result.LoadPngFromBuffer(convertedData);
        if ( error != Error.Ok ) {
            string errorName = Enum.GetName(typeof(Error) , error);
            EmitSignal(SignalName.ImageLoadFailed , errorName);
            return null;
        }
        return result;
    }
#nullable disable

    public void Backup()
    {
        string originalSaveContents;
        string backupName = $"backup_{DateTime.Now:MMddyyyy_HHmmss}.jefftube";
        string backupPath = $"{savePath}/{backupName}";
        using ( FileAccess originalSave = FileAccess.Open(savePath , FileAccess.ModeFlags.Read) ) {
            originalSaveContents = OS.GetName() == "Windows" ? originalSave.GetAsText() : originalSave.GetAsText(true);
        }
        using ( FileAccess saveBackup = FileAccess.Open(backupPath , FileAccess.ModeFlags.Write) ) {
            saveBackup.StoreString(originalSaveContents);
        }
    }
    public void Save()
    {
        
        string json = JsonConvert.SerializeObject(Parts);
        using (FileAccess newSave = FileAccess.Open(savePath,FileAccess.ModeFlags.Write) ) {
            newSave.StoreString(json);
        }
    }
    public Dictionary<byte , AvatarPart> Load()
    {
        Backup();
        Dictionary<byte , AvatarPart> result;
        using ( FileAccess file = FileAccess.Open(savePath,FileAccess.ModeFlags.Read) ) {
            string json = OS.GetName() == "Windows" ? file.GetAsText() : file.GetAsText(true);
            result = JsonConvert.DeserializeObject<Dictionary<byte , AvatarPart>>(json);
        }
        return result;
    }
    private void event_ImageLoadFailed(string error)
    {
        if( Enum.TryParse(error, out Error result) ) {
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
}