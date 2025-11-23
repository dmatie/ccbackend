using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetFileUploadedQueryHandler(
    IDisbursementDocumentService disbursementDocumentService,
    IDisbursementRepository disbursementRepository) : IRequestHandler<GetFileUploadedQuery, FileUploadedDto>
{
    public async Task<FileUploadedDto> Handle(GetFileUploadedQuery request, CancellationToken cancellationToken)
    {
        bool exists = await disbursementRepository
            .ReferenceExist(request.ReferenceNumber, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException("ERR.Disbursement.ReferenceNotFound");
        }

        bool fileExists = await disbursementRepository
            .FileNameExist(request.ReferenceNumber, request.FileName, cancellationToken);
        if (!fileExists)
        {
            throw new NotFoundException("ERR.Disbursement.FileNotFound");
        }

        FileDownloaded? fileDownloaded = await disbursementDocumentService
            .DownloadAttachDocumentsAsync(request.ReferenceNumber, request.FileName, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.FileNotFound");

        return new FileUploadedDto
        {
            FileName = fileDownloaded.FileName,
            FileContent = fileDownloaded.FileContent,
            ContentType = fileDownloaded.ContentType
        };
    }
}
