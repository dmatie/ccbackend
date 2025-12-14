using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.OtherDocumentQrs;

public sealed class GetOtherDocumentTypesQueryHandler : IRequestHandler<GetOtherDocumentTypesQuery, List<OtherDocumentTypeDto>>
{
    private readonly IOtherDocumentTypeRepository _otherDocumentTypeRepository;
    private readonly IMapper _mapper;

    public GetOtherDocumentTypesQueryHandler(
        IOtherDocumentTypeRepository otherDocumentTypeRepository,
        IMapper mapper)
    {
        _otherDocumentTypeRepository = otherDocumentTypeRepository;
        _mapper = mapper;
    }

    public async Task<List<OtherDocumentTypeDto>> Handle(GetOtherDocumentTypesQuery request, CancellationToken cancellationToken)
    {
        var otherDocumentTypes = await _otherDocumentTypeRepository.GetActiveAsync();

        return _mapper.Map<List<OtherDocumentTypeDto>>(otherDocumentTypes);
    }
}
