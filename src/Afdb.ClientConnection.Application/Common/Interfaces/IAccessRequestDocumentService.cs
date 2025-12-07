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
}
