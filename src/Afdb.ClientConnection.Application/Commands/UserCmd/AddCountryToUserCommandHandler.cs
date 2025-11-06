using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using MediatR;


namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed class AddCountryToUserCommandHandler : IRequestHandler<AddCountryToUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly ICountryAdminRepository _countryAdminRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly ICurrentUserService _currentUserService;


    public AddCountryToUserCommandHandler(IUserRepository userRepository,
        ICountryAdminRepository countryAdminRepository,
        ICountryRepository countryRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _countryAdminRepository = countryAdminRepository;
        _currentUserService = currentUserService;
        _countryRepository = countryRepository;
    }

    public async Task<bool> Handle(AddCountryToUserCommand request, CancellationToken cancellationToken)
    {

        // Vérifier que l'utilisateur actuel peut approuver
        if (!_currentUserService.IsInRole("Admin"))
        {
            throw new ForbiddenAccessException("ERR.General.NotAuthorize");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId) 
            ?? throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("UserId", "ERR.User.NotFound")
                });

        var exist = await _countryAdminRepository.ExistsAsync(request.UserId, request.CountryIds);
        if (exist)
            throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("UserId", "ERR.User.CountryAlreadyExist")
                });

        var allExist = await _countryRepository.AllExistAsync(request.CountryIds);
        if (!allExist)
            throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("CountryIds", "ERR.User.CountryNotFound")
                });


        List<CountryAdmin> countries = [];
       
        foreach (var countryId in request.CountryIds)
        {
            var countryAdmin = new CountryAdmin(new CountryAdminNewParam
            {
                CountryId = countryId,
                IsActive = true
            });
            countries.Add(countryAdmin);
        }

        user.AddCountries(countries, _currentUserService.UserId ?? "System");

        await _userRepository.UpdateAsync(user);

        return true;
    }
}