using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed class GetSignedFormUploadedQueryHandler(
    IAccessRequestDocumentService accessRequestDocument,
    IAccessRequestRepository accessRequestRepository ) : IRequestHandler<GetSignedFormUploadedQuery, FileUploadedDto>
{
    public async Task<FileUploadedDto> Handle(GetSignedFormUploadedQuery request, CancellationToken cancellationToken)
    {
        var accessRequest = await accessRequestRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException("ERR.AccessRequest.RequestNotExist");

        if(accessRequest.Documents.Count == 0)
            throw new NotFoundException("ERR.AccessRequest.SignedFormNotExist");


        var signedForm = accessRequest.Documents.FirstOrDefault() 
            ?? throw new NotFoundException("ERR.AccessRequest.SignedFormNotExist");

        FileDownloaded? fileDownloaded = await accessRequestDocument
            .DownloadDocumentAsync(accessRequest.Code, signedForm.FileName, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.FileNotFound");

        return new FileUploadedDto
        {
            FileName = fileDownloaded.FileName,
            FileContent = fileDownloaded.FileContent,
            ContentType = fileDownloaded.ContentType
        };
    }
}
