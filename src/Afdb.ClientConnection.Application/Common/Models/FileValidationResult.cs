namespace Afdb.ClientConnection.Application.Common.Models;

public sealed class FileValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public string FileName { get; set; } = string.Empty;

    public static FileValidationResult Success(string fileName)
    {
        return new FileValidationResult
        {
            IsValid = true,
            FileName = fileName
        };
    }

    public static FileValidationResult Failure(string fileName, params string[] errors)
    {
        return new FileValidationResult
        {
            IsValid = false,
            FileName = fileName,
            Errors = errors.ToList()
        };
    }
}
