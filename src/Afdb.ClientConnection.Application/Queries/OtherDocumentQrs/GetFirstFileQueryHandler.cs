using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed class GetFirstFileQueryHandler(
    IOtherDocumentService otherDocumentService,
    IOtherDocumentRepository otherDocumentRepository) : IRequestHandler<GetFirstFileQuery, FileUploadedDto>
{
    public async Task<FileUploadedDto> Handle(GetFirstFileQuery request, CancellationToken cancellationToken)
    {
        var otherDocument = await otherDocumentRepository
            .GetByIdWithFilesAsync(request.OtherDocumentId, cancellationToken);

        if (otherDocument == null)
        {
            throw new NotFoundException("ERR.OtherDocument.NotFound");
        }

        if (otherDocument.Files == null || !otherDocument.Files.Any())
        {
            throw new NotFoundException("ERR.OtherDocument.NoFilesFound");
        }

        var firstFile = otherDocument.Files.OrderBy(f => f.UploadedAt).First();

        FileDownloaded? fileDownloaded = await otherDocumentService
            .DownloadFileAsync(request.OtherDocumentId, firstFile.FileName, cancellationToken)
            ?? throw new NotFoundException("ERR.OtherDocument.FileNotFound");

        return new FileUploadedDto
        {
            FileName = fileDownloaded.FileName,
            FileContent = fileDownloaded.FileContent,
            ContentType = fileDownloaded.ContentType
        };
    }
}
