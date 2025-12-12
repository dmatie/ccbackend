using Afdb.ClientConnection.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IAccessRequestDocumentService
{
    Task UploadAndAttachDocumentAsync(
        AccessRequest accessRequest,
        IFormFile document,
        CancellationToken cancellationToken = default);

    Task<FileDownloaded?> DownloadDocumentAsync(
        string code,
        string fileName,
        CancellationToken cancellationToken = default);

    Task<(string frenchUrl, string idFr, string englishUrl, string idEn)> GenerateAndUploadAuthorizationFormsAsync(
        string code,
        string firstName,
        string lastName,
        string email,
        string functionName,
        string functionNameFr,
        List<string> projects,
        CancellationToken cancellationToken = default);
}
