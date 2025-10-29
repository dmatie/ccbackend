using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed class CreateUserWithCountriesCommandHandler : IRequestHandler<CreateUserWithCountriesCommand, UserWithCountriesDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly ICountryAdminRepository _countryAdminRepository;
    private readonly IGraphService _graphService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateUserWithCountriesCommandHandler> _logger;

    public CreateUserWithCountriesCommandHandler(
        IUserRepository userRepository,
        ICountryRepository countryRepository,
        ICountryAdminRepository countryAdminRepository,
        IGraphService graphService,
        ICurrentUserService currentUserService,
        ILogger<CreateUserWithCountriesCommandHandler> logger)
    {
        _userRepository = userRepository;
        _countryRepository = countryRepository;
        _countryAdminRepository = countryAdminRepository;
        _graphService = graphService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<UserWithCountriesDto> Handle(CreateUserWithCountriesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating user with countries: Email={Email}, Role={Role}, CountryCount={CountryCount}",
            request.Email,
            request.Role,
            request.CountryIds.Count);

        var currentUser = _currentUserService.UserId ?? "System";

        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User already exists with email: {Email}", request.Email);
            throw new ValidationException(new ExceptionContent
            {
                Code = "ERR.User.AlreadyExists",
                Message = $"User with email {request.Email} already exists in the system"
            });
        }

        var userExistsInAzureAd = await _graphService.UserExistsAsync(request.Email, cancellationToken);
        if (!userExistsInAzureAd)
        {
            _logger.LogWarning("User not found in Azure AD: {Email}", request.Email);
            throw new ValidationException(new ExceptionContent
            {
                Code = "ERR.User.NotFoundInAzureAd",
                Message = $"User with email {request.Email} does not exist in Azure AD"
            });
        }

        if (request.Role == UserRole.DO || request.Role == UserRole.DA)
        {
            if (request.CountryIds == null || request.CountryIds.Count == 0)
            {
                _logger.LogWarning("No countries provided for role {Role}", request.Role);
                throw new ValidationException(new ExceptionContent
                {
                    Code = "ERR.User.CountriesRequired",
                    Message = $"At least one country must be assigned for {request.Role} role"
                });
            }

            var allCountriesExist = await _countryRepository.AllExistAsync(request.CountryIds, cancellationToken);
            if (!allCountriesExist)
            {
                _logger.LogWarning("One or more countries do not exist");
                throw new ValidationException(new ExceptionContent
                {
                    Code = "ERR.Country.NotFound",
                    Message = "One or more country IDs do not exist"
                });
            }
        }

        var azureAdUser = await GetAzureAdUserDetailsAsync(request.Email, cancellationToken);

        var user = new User(
            email: request.Email,
            firstName: azureAdUser.FirstName,
            lastName: azureAdUser.LastName,
            role: request.Role,
            entraIdObjectId: azureAdUser.EntraIdObjectId,
            createdBy: currentUser);

        var createdUser = await _userRepository.AddAsync(user);

        _logger.LogInformation("User created with ID: {UserId}", createdUser.Id);

        List<Country> assignedCountries = [];

        if (request.Role == UserRole.DO || request.Role == UserRole.DA)
        {
            var countryAdmins = request.CountryIds.Select(countryId => new CountryAdmin(new CountryAdminNewParam
            {
                UserId = createdUser.Id,
                CountryId = countryId,
                IsActive = true
            })).ToList();

            await _countryAdminRepository.AddRangeAsync(countryAdmins, cancellationToken);

            assignedCountries = await _countryRepository.GetByIdsAsync(request.CountryIds, cancellationToken);

            _logger.LogInformation(
                "Assigned {CountryCount} countries to user {UserId}",
                assignedCountries.Count,
                createdUser.Id);
        }

        return new UserWithCountriesDto
        {
            Id = createdUser.Id,
            Email = createdUser.Email,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            FullName = createdUser.FullName,
            Role = createdUser.Role.ToString(),
            IsActive = createdUser.IsActive,
            EntraIdObjectId = createdUser.EntraIdObjectId,
            Countries = assignedCountries.Select(c => new CountryDto
            {
                Id = c.Id,
                Name = c.Name,
                NameFr = c.NameFr,
                Code = c.Code,
                IsActive = c.IsActive
            }).ToList()
        };
    }

    private async Task<AzureAdUserDetails> GetAzureAdUserDetailsAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var searchResults = await _graphService.SearchUsersAsync(email, 1, cancellationToken);

            var user = searchResults.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _logger.LogWarning("Could not find user details in Azure AD for: {Email}", email);
                throw new NotFoundException(new ExceptionContent
                {
                    Code = "ERR.User.NotFoundInAzureAd",
                    Message = $"Could not retrieve user details from Azure AD for {email}"
                });
            }

            var nameParts = user.DisplayName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts.Length > 0 ? nameParts[0] : email.Split('@')[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

            return new AzureAdUserDetails
            {
                EntraIdObjectId = user.Id,
                FirstName = firstName,
                LastName = lastName
            };
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            _logger.LogError(ex, "Error retrieving user details from Azure AD for: {Email}", email);
            throw new ServerErrorException(new ExceptionContent
            {
                Code = "ERR.AzureAd.RetrievalError",
                Message = "Failed to retrieve user information from Azure AD"
            });
        }
    }

    private sealed record AzureAdUserDetails
    {
        public string EntraIdObjectId { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
    }
}
