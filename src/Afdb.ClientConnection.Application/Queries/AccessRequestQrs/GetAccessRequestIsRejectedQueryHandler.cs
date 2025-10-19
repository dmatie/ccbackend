using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.AccessRequestQrs;

public sealed class GetAccessRequestIsRejectedQueryHandler(
    IAccessRequestRepository accessRequestRepository) : IRequestHandler<GetAccessRequestIsRejectedQuery, bool>
{
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;

    public async Task<bool> Handle(GetAccessRequestIsRejectedQuery request,
        CancellationToken cancellationToken)
    {
        bool hasStatus = await _accessRequestRepository
            .EmailHasStatusRequestAsync(request.Email, Domain.Enums.RequestStatus.Rejected);

        return hasStatus;
    }
}