using Afdb.ClientConnection.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IOtherDocumentService
{
    Task UploadAndAttachFilesAsync(
        OtherDocument otherDocument,
        IFormFileCollection files,
        CancellationToken cancellationToken = default);

    Task<FileDownloaded?> DownloadFileAsync(
        Guid otherDocumentId,
        string fileName,
        CancellationToken cancellationToken = default);
}
