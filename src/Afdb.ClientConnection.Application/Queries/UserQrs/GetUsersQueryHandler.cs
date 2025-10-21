using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Queries.UserQrs;

public sealed class GetUsersQueryHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // Seuls les Admin peuvent lister tous les utilisateurs
        if (!_currentUserService.IsInRole("Admin"))
        {
            throw new UnauthorizedAccessException("ERR.General.NotAuthorize");
        }

        var users = await _userRepository.GetActiveUsersAsync();

        // Filtrer par rôle si spécifié
        if (request.Role.HasValue)
        {
            users = users.Where(u => u.Role == request.Role.Value);
        }

        // Filtrer par statut actif si spécifié
        if (request.IsActive.HasValue)
        {
            users = users.Where(u => u.IsActive == request.IsActive.Value);
        }

        // Filtrer par organisation si spécifié
        if (!string.IsNullOrEmpty(request.OrganizationName))
        {
            users = users.Where(u => u.OrganizationName != null &&
                u.OrganizationName.Contains(request.OrganizationName, StringComparison.OrdinalIgnoreCase));
        }

        // Ordonner par nom
        users = users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);

        // Pagination
        var totalCount = users.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var pagedUsers = users
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<UserDto>>(pagedUsers);

        return new GetUsersResponse
        {
            Users = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }
}