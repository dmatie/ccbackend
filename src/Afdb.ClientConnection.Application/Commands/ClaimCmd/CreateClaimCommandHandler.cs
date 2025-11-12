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
    ICurrentUserService currentUserService,
    IAccessRequestRepository accessRequestRepository,
    ICountryAdminRepository countryAdminRepository,
    IMapper mapper) : IRequestHandler<CreateClaimCommand, CreateClaimResponse>
{
    private readonly IClaimRepository _claimRepository = claimRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICountryRepository _countryRepository = countryRepository;
    private readonly IClaimTypeRepository _claimTypeRepository = claimTypeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IGraphService _graphService = graphService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly ICountryAdminRepository _countryAdminRepository = countryAdminRepository;

    public async Task<CreateClaimResponse> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var claimType = await _claimTypeRepository.GetByIdAsync(request.ClaimTypeId);
        if (claimType == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("ClaimTypeId", "ERR.Claim.ClaimTypeNotExist")
            });

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        var accessRequest = await _accessRequestRepository.GetByEmailAsync(user.Email) ??
            throw new NotFoundException("ERR.General.AccessRequestNotFound");

        Country? country = null;
        if (!accessRequest.CountryId.HasValue)
        {
            country = await _countryRepository.GetDefaultCountryAsync() ?? throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("UserId", "ERR.Claim.RelatedCountryNotExist")
             });
        }
        else
        {
            country = await _countryRepository.GetByIdAsync(accessRequest.CountryId.Value) ?? throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("UserId", "ERR.Claim.RelatedCountryNotExist")
             });
        }

        string[] fifcAdmins = (await _graphService.GetFifcAdmin(cancellationToken)).ToArray() ?? [];

        var countryAdmins = await _countryAdminRepository.GetByCountryIdAsync(country.Id, cancellationToken);

        string[] assignTo;

        if (countryAdmins != null && countryAdmins.Any())
        {
            assignTo = countryAdmins.Select(ca => ca.User!.Email).ToArray();
        }
        else
        {
            assignTo = fifcAdmins;
        }

        var claimNewParam = new ClaimNewParam
        {
            ClaimTypeId = request.ClaimTypeId,
            UserId = user.Id,
            CountryId = country.Id,
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
