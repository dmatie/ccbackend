using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.ProjectQrs;

public sealed class GetProjectLoanNumberQueryHandler(ISapService sapService, IMapper mapper) :
    IRequestHandler<GetProjectLoanNumberQuery, GetProjectLoanNumberResponse>
{
    private readonly ISapService _sapService = sapService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetProjectLoanNumberResponse> Handle(GetProjectLoanNumberQuery request, CancellationToken cancellationToken)
    {
        var projectLoans = (await _sapService.GetProjectLoanNumbersAsync(request.sapCode, cancellationToken)).ToList();

        List<ProjectLoanNumberDto> dtos = _mapper.Map<List<ProjectLoanNumberDto>>(projectLoans);

        return new GetProjectLoanNumberResponse
        {
            ProjectLoanNumbers = dtos,
            TotalCount = projectLoans.Count
        };
    }
}