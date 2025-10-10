using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed class AddClaimResponseCommandHandler : IRequestHandler<AddClaimResponseCommand, ClaimDto>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AddClaimResponseCommandHandler(
        IClaimRepository claimRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _claimRepository = claimRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ClaimDto> Handle(AddClaimResponseCommand request, CancellationToken cancellationToken)
    {
        var claim = await _claimRepository.GetByIdAsync(request.ClaimId);
        if (claim == null)
            throw new ValidationException(nameof(request.ClaimId), "Claim not found");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new ValidationException(nameof(request.UserId), "User not found");

        var claimProcess = new ClaimProcess(
            request.ClaimId,
            request.UserId,
            request.Comment,
            user
        );

        claim.AddProcess(claimProcess, request.Status);

        await _claimRepository.UpdateAsync(claim);

        var updatedClaim = await _claimRepository.GetByIdAsync(request.ClaimId);

        return _mapper.Map<ClaimDto>(updatedClaim);
    }
}
