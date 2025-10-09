using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public class GetAccessRequestByEmailQueryHandler
    (IAccessRequestRepository repository, IMapper mapper) :
    IRequestHandler<GetAccessRequestByEmailQuery, GetAccessRequestByEmailResponse>
{

    public async Task<GetAccessRequestByEmailResponse> Handle(GetAccessRequestByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var accessRequest = await repository.GetByEmailAsync(request.Email);
        if (accessRequest == null)
            throw new NotFoundException("ERR.AccessRequest.NotExistEmail");

        var dto = mapper.Map<AccessRequestDto>(accessRequest);

        return new GetAccessRequestByEmailResponse { AccessRequest = dto };
    }
}