using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed class CreateUserInternalUserCommandHandler : IRequestHandler<CreateUserInternalUserCommand, CreateUserInternalUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IGraphService _graphService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateUserInternalUserCommandHandler> _logger;
    private readonly IMapper _mapper;


    public CreateUserInternalUserCommandHandler(
        IUserRepository userRepository,
        ICountryRepository countryRepository,
        ICountryAdminRepository countryAdminRepository,
        IGraphService graphService,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<CreateUserInternalUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _countryRepository = countryRepository;
        _graphService = graphService;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateUserInternalUserResponse> Handle(CreateUserInternalUserCommand command, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.UserId ?? "System";

        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User already exists with email: {Email}", command.Email);
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.User.AlreadyExists")
            });
        }

        var userExistsInAzureAd = await _graphService.UserExistsAsync(command.Email, cancellationToken);
        if (!userExistsInAzureAd)
        {
            _logger.LogWarning("User not found in Azure AD: {Email}", command.Email);
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.User.NotFoundInAD")
            });
        }

        if (command.Role == UserRole.DO || command.Role == UserRole.DA)
        {
            if (command.CountryIds == null || command.CountryIds.Count == 0)
            {
                _logger.LogWarning("No countries provided for role {Role}", command.Role);
                throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("CountryId", "ERR.User.CountriesRequired")
                });
            }

            var allCountriesExist = await _countryRepository.AllExistAsync(command.CountryIds, cancellationToken);
            if (!allCountriesExist)
            {
                _logger.LogWarning("One or more countries do not exist");
                throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("CountryId", "ERR.Country.NotFound")
                });
            }
        }

        var azureAdUser = await _graphService.GetAzureAdUserDetailsAsync(command.Email, cancellationToken) ??
               throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("Email", "ERR.User.EmainNotExistInAd")
                });


        List<CountryAdmin> countries = [];

        if (command.Role == UserRole.DO || command.Role == UserRole.DA)
        {

            command.CountryIds?.ForEach(countryId =>
            {
                countries.Add(new CountryAdmin(new CountryAdminNewParam
                {
                    CountryId = countryId,
                    IsActive = true
                }));
            });
        }

        User user = new (new UserNewParam
        {
            Email = command.Email,
            FirstName = azureAdUser.FirstName,
            LastName = azureAdUser.LastName,
            Role = command.Role,
            EntraIdObjectId = azureAdUser.Id,
            CreatedBy = currentUser,
            OrganizationName = "AFDB",
            Countries = countries
        });

        var createdUser = await _userRepository.AddInternalAsync(user, azureAdUser, command.Role,cancellationToken);

        UserDto userDto = _mapper.Map<UserDto>(createdUser);

        return new CreateUserInternalUserResponse
        {
            User = userDto,
            Message = "MSG.User.InternalCreated"
        };
    }
}
