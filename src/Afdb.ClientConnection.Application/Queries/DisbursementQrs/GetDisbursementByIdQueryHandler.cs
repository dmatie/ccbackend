using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public sealed class GetDisbursementByIdQueryHandler(
    IDisbursementRepository disbursementRepository,
    IMapper mapper) : IRequestHandler<GetDisbursementByIdQuery, DisbursementDto>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<DisbursementDto> Handle(GetDisbursementByIdQuery request, CancellationToken cancellationToken)
    {
        var disbursement = await _disbursementRepository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new NotFoundException("ERR.Disbursement.NotFound");

        return _mapper.Map<DisbursementDto>(disbursement);
    }
}
