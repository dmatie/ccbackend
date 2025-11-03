using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetAllDisbursementsQueryHandler(
    IDisbursementRepository disbursementRepository,
    IUserContextService userContextService,
    IMapper mapper) : IRequestHandler<GetAllDisbursementsQuery, IEnumerable<DisbursementDto>>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DisbursementDto>> Handle(GetAllDisbursementsQuery request, CancellationToken cancellationToken)
    {
        var userContext = _userContextService.GetUserContext();

        var disbursements = await _disbursementRepository.GetAllAsync(userContext, cancellationToken);
        return _mapper.Map<IEnumerable<DisbursementDto>>(disbursements);
    }
}
