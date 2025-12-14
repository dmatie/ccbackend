using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed class GetOtherDocumentByIdQueryHandler : IRequestHandler<GetOtherDocumentByIdQuery, OtherDocumentDto?>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository;
    private readonly IMapper _mapper;

    public GetOtherDocumentByIdQueryHandler(
        IOtherDocumentRepository otherDocumentRepository,
        IMapper mapper)
    {
        _otherDocumentRepository = otherDocumentRepository;
        _mapper = mapper;
    }

    public async Task<OtherDocumentDto?> Handle(GetOtherDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.OtherDocumentId == Guid.Empty)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("OtherDocumentId", "ERR.OtherDocument.IdRequired")
            });
        }

        var otherDocument = await _otherDocumentRepository.GetByIdWithFilesAsync(
            request.OtherDocumentId,
            cancellationToken);

        if (otherDocument == null)
        {
            throw new NotFoundException("ERR.OtherDocument.NotFound");
        }

        return _mapper.Map<OtherDocumentDto>(otherDocument);
    }
}
