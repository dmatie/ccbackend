using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed class AddClaimResponseCommandHandler(
    IClaimRepository claimRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<AddClaimResponseCommand, AddClaimResponseResponse>
{
    private readonly IClaimRepository _claimRepository = claimRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<AddClaimResponseResponse> Handle(AddClaimResponseCommand request, CancellationToken cancellationToken)
    {
        var claim = await _claimRepository.GetByIdAsync(request.ClaimId);
        if (claim == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Claim", "ERR.Claim.ClaimNotFound")
            });

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email);
        if (user == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Claim", "ERR.Claim.UserNotFound")
            });

        var claimProcess = new ClaimProcess(new ClaimProcessNewParam
        {
            ClaimId = claim.Id,
            UserId = user.Id,
            Comment = request.Comment,
            CreatedBy = _currentUserService.Email,
            Status = (ClaimStatus)request.Status,
            User = user,
            CreatedAt = DateTime.UtcNow
        });

        claim.AddProcess(claimProcess, user);

        await _claimRepository.UpdateAsync(claim);

        var updatedClaim = await _claimRepository.GetByIdAsync(request.ClaimId);

        return new AddClaimResponseResponse
        {
            Claim = _mapper.Map<ClaimDto>(updatedClaim),
            Message = "MSG.Claim.ResponseAdded"
        };
    }
}
