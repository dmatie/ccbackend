using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditService _auditService;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IAuditService auditService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _auditService = auditService;
        _mapper = mapper;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        // Vérifier les permissions - seuls Admin peuvent créer des utilisateurs
        if (!_currentUserService.IsInRole("Admin"))
        {
            throw new ForbiddenAccessException("ERR.General.NotAuthorize");
        }

        // Vérifier si l'utilisateur existe déjà par email
        bool existingUserByEmail = await _userRepository.EmailExistsAsync(request.Email);
        if (existingUserByEmail)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("Email", "ERR.User.EmailAlreadyExists")
            });
        }

        // Vérifier si l'utilisateur existe déjà par Entra ID (si fourni)
        if (!string.IsNullOrEmpty(request.EntraIdObjectId))
        {
            var existingUserByEntraId = await _userRepository.GetByEntraIdObjectIdAsync(request.EntraIdObjectId);
            if (existingUserByEntraId != null)
            {
                throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("EntraIdObjectId", "ERR.User.EntraIdAlreadyExist")
                });
            }
        }

        // Créer l'utilisateur
        User user;
        if (request.Role == UserRole.ExternalUser)
        {
            // Pour les utilisateurs externes, l'Entra ID est obligatoire
            if (string.IsNullOrEmpty(request.EntraIdObjectId))
            {
                throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("EntraIdObjectId", "ERR.User.MandatoryEntraId")
                });
            }

            user = User.CreateExternalUser(
                request.Email,
                request.FirstName,
                request.LastName,
                request.OrganizationName!,
                request.EntraIdObjectId,
                _currentUserService.UserId);
        }
        else
        {
            // Pour les utilisateurs internes
            user = new User(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Role,
                request.EntraIdObjectId ?? string.Empty,
                _currentUserService.UserId,
                request.OrganizationName);
        }

        // Sauvegarder
        await _userRepository.AddAsync(user);

        // Logger l'audit
        await _auditService.LogAsync(
            nameof(User),
            user.Id,
            "Create",
            null,
            newValues: System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role.ToString(),
                OrganizationName = request.OrganizationName
            }),
            cancellationToken);

        var dto = _mapper.Map<UserDto>(user);

        return new CreateUserResponse
        {
            User = dto,
            Message = "MSG.User.Created"
        };
    }
}