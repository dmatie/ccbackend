using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetAllDisbursementsQueryHandler(
    IDisbursementRepository disbursementRepository,
    IMapper mapper) : IRequestHandler<GetAllDisbursementsQuery, IEnumerable<DisbursementDto>>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DisbursementDto>> Handle(GetAllDisbursementsQuery request, CancellationToken cancellationToken)
    {
        var disbursements = await _disbursementRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<DisbursementDto>>(disbursements);
    }
}
