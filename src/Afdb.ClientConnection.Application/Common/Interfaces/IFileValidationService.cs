using Afdb.ClientConnection.Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IFileValidationService
{
    FileValidationResult ValidateFile(IFormFile file);

    Task<FileValidationResult> ValidateFileAsync(IFormFile file);

    List<FileValidationResult> ValidateFiles(IEnumerable<IFormFile> files);

    Task<List<FileValidationResult>> ValidateFilesAsync(IEnumerable<IFormFile> files);
}
