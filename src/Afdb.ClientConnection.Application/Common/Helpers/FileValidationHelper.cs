using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Common.Helpers;

public static class FileValidationHelper
{
    public static async Task ValidateAndThrowAsync(
        this IFileValidationService fileValidationService,
        IFormFile file,
        string propertyName = "File")
    {
        var result = await fileValidationService.ValidateFileAsync(file);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(error =>
                new FluentValidation.Results.ValidationFailure(propertyName, error)
            ).ToArray();

            throw new ValidationException(errors);
        }
    }

    public static async Task ValidateAndThrowAsync(
        this IFileValidationService fileValidationService,
        IEnumerable<IFormFile> files,
        string propertyName = "Documents")
    {
        var validationResults = await fileValidationService.ValidateFilesAsync(files);
        var invalidFiles = validationResults.Where(r => !r.IsValid).ToList();

        if (invalidFiles.Count > 0)
        {
            var errors = invalidFiles.SelectMany(f =>
                f.Errors.Select(error => new FluentValidation.Results.ValidationFailure(
                    $"{propertyName}[{f.FileName}]",
                    error
                ))
            ).ToArray();

            throw new ValidationException(errors);
        }
    }
}
