namespace Afdb.ClientConnection.Infrastructure.Settings;

public sealed class FileUploadSettings
{
    public const string SectionName = "FileUpload";

    public int MaxFileSizeInMB { get; set; } = 10;
    public List<string> AllowedExtensions { get; set; } = new();
    public List<string> AllowedMimeTypes { get; set; } = new();

    public long MaxFileSizeInBytes => MaxFileSizeInMB * 1024 * 1024;
}
