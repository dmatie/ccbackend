using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Afdb.ClientConnection.Infrastructure.Services;

public sealed class FileValidationService : IFileValidationService
{
    private readonly FileUploadSettings _settings;

    public FileValidationService(IOptions<FileUploadSettings> settings)
    {
        _settings = settings.Value;
    }

    public FileValidationResult ValidateFile(IFormFile file)
    {
        if (file == null)
        {
            return FileValidationResult.Failure("Unknown", "ERR.FILE.NULL");
        }

        var errors = new List<string>();
        var fileName = file.FileName;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            errors.Add("ERR.FILE.NAME_EMPTY");
        }

        if (file.Length == 0)
        {
            errors.Add("ERR.FILE.EMPTY");
        }

        if (file.Length > _settings.MaxFileSizeInBytes)
        {
            errors.Add($"ERR.FILE.SIZE_EXCEEDED");
        }

        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_settings.AllowedExtensions.Contains(extension))
        {
            errors.Add($"ERR.FILE.EXTENSION_NOT_ALLOWED");
        }

        var contentType = file.ContentType?.ToLowerInvariant();
        if (string.IsNullOrEmpty(contentType) || !_settings.AllowedMimeTypes.Contains(contentType))
        {
            errors.Add($"ERR.FILE.MIMETYPE_NOT_ALLOWED");
        }

        if (errors.Count > 0)
        {
            return FileValidationResult.Failure(fileName, errors.ToArray());
        }

        return FileValidationResult.Success(fileName);
    }

    public Task<FileValidationResult> ValidateFileAsync(IFormFile file)
    {
        return Task.FromResult(ValidateFile(file));
    }

    public List<FileValidationResult> ValidateFiles(IEnumerable<IFormFile> files)
    {
        if (files == null)
        {
            return new List<FileValidationResult>();
        }

        return files.Select(ValidateFile).ToList();
    }

    public Task<List<FileValidationResult>> ValidateFilesAsync(IEnumerable<IFormFile> files)
    {
        return Task.FromResult(ValidateFiles(files));
    }
}
