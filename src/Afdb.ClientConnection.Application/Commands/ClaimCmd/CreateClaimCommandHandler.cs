using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed class CreateClaimCommandHandler(
    IClaimRepository claimRepository,
    IUserRepository userRepository,
    ICountryRepository countryRepository,
    IClaimTypeRepository claimTypeRepository,
    IGraphService graphService,
    IMapper mapper) : IRequestHandler<CreateClaimCommand, CreateClaimResponse>
{
    private readonly IClaimRepository _claimRepository = claimRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICountryRepository _countryRepository = countryRepository;
    private readonly IClaimTypeRepository _claimTypeRepository = claimTypeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IGraphService _graphService = graphService;


    public async Task<CreateClaimResponse> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var claimType = await _claimTypeRepository.GetByIdAsync(request.ClaimTypeId);
        if (claimType == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("ClaimTypeId", "ERR.Claim.ClaimTypeNotExist")
            });

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("ERR.General.UserNotFound");

        var country = await _countryRepository.GetByIdAsync(request.CountryId);
        if (country == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("UserId", "ERR.Claim.RelatedCountryNotExist")
            });

        string [] fifcAdmins = (await _graphService.GetFifcAdmin(cancellationToken)).ToArray() ?? [];  
        
        List<UserRole> Roles= new List<UserRole> { UserRole.DA,UserRole.DO };

        IEnumerable<User> usersAssignTo = await _userRepository.GetActiveUsersByRolesAsync(Roles);

        if (usersAssignTo == null || !usersAssignTo.Any())
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("AssignTo", "ERR.Claim.NoUserToAssign")
            });

        string[] assignTo= [.. usersAssignTo.Select(u=>u.Email)];

        var claimNewParam = new ClaimNewParam
        {
            ClaimTypeId = request.ClaimTypeId,
            UserId = request.UserId,
            CountryId = request.CountryId,
            Comment = request.Comment,
            User = user,
            Country = country,
            ClaimType = claimType,
            AssignTo = assignTo,
            AssignCc = fifcAdmins!
        };

        var claim = new Claim(claimNewParam);

        var createdClaim = await _claimRepository.AddAsync(claim);

        return new CreateClaimResponse
        {
            Claim = _mapper.Map<ClaimDto>(createdClaim),
            Message = "MSG.CLAIM.CREATED_SUCCESS"
        };
    }
}
