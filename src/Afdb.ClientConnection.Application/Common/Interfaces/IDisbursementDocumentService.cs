using Afdb.ClientConnection.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IDisbursementDocumentService
{
    Task UploadAndAttachDocumentsAsync(
        Disbursement disbursement,
        IEnumerable<IFormFile> documents,
        CancellationToken cancellationToken = default);
}
