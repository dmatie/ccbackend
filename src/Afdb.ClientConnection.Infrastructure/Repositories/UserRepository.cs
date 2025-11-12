using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly ClientConnectionDbContext _context;
    private ILogger<UserRepository> _logger;
    private readonly IGraphService _graphService;


    public UserRepository(ClientConnectionDbContext context, 
        IGraphService graphService, ILogger<UserRepository> logger)
    {
        _context = context;
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Users
            .Include(u => u.CountryAdmins).ThenInclude(ca => ca.Country)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (entity == null) 
        {
            return null;
        }

        User user = DomainMappings.MapUserToDomain(entity);
        
        foreach (var countryAdmin in user.Countries)
        {
            CountryAdminEntity? entityCountry = entity.CountryAdmins.FirstOrDefault(c=>c.CountryId==countryAdmin.CountryId);
            if (entityCountry != null)
                countryAdmin.SetCountry(DomainMappings.MapCountry(entityCountry.Country));
        }


        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));

        return entity == null ? null : DomainMappings.MapUserToDomain(entity);
    }

    public async Task<User?> GetByEntraIdObjectIdAsync(string entraIdObjectId)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.EntraIdObjectId == entraIdObjectId);

        return entity == null ? null : DomainMappings.MapUserToDomain(entity);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        var entities = await _context.Users
            .Where(u => u.IsActive)
            .ToListAsync();

        return entities.Select(DomainMappings.MapUserToDomain);
    }

    public async Task<IEnumerable<User>> GetActiveUsersByRolesAsync(List<UserRole> roles)
    {
        var entities = await _context.Users
            .Where(u => u.IsActive && roles.Contains(u.Role))
            .ToListAsync();

        return entities.Select(DomainMappings.MapUserToDomain);
    }

    public async Task<IEnumerable<User>> GetActiveInternalUsersAsync()
    {
        var roles = new List<UserRole> { UserRole.Admin, UserRole.DA, UserRole.DO };

        var entities = await _context.Users
            .Where(u => u.IsActive && roles.Contains(u.Role))
            .ToListAsync();

        return entities.Select(DomainMappings.MapUserToDomain);
    }

    public async Task<IEnumerable<User>> GetUsersAdminByCountry(Guid countryId, CancellationToken cancellationToken)
    {
        var entities = await _context.Users
            .Include(u => u.CountryAdmins)
            .Where(u => u.IsActive && u.CountryAdmins.Any(ca => ca.CountryId == countryId))
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapUserToDomain);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var entity = EntityMappings.MapUserToEntity(user);
        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return DomainMappings.MapUserToDomain(entity);
    }

    public async Task<User> AddInternalAsync(User user, AzureAdUserDetails adUserDetails, UserRole userRole, 
        CancellationToken cancellationToken= default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);

        try
        {
            var entity = EntityMappings.MapUserToEntity(user);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            await _graphService.AddUserToGroupByNameAsync(adUserDetails.Id, userRole, cancellationToken);

            await _graphService.AssignAppRoleToUserByRoleNameAsync(adUserDetails.Id, userRole, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return DomainMappings.MapUserToDomain(entity);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(
                ex,
                "Error When creating user {User}",
                user);

            await _graphService.RemoveUserFromGroupByNameAsync(adUserDetails.Id, userRole, cancellationToken);
            await _graphService.RemoveAppRoleFromUserByRoleNameAsync(adUserDetails.Id, userRole, cancellationToken);

            throw new InvalidOperationException("ERR.User.FailToCreate");
        }
    }


    public async Task UpdateAsync(User user)
    {
        var entity = await _context.Users
            .Include(u => u.CountryAdmins)
            .AsTracking()
            .FirstOrDefaultAsync(u=>u.Id== user.Id);

        if (entity != null)
        {
            EntityMappings.UpdateUserEntityFromDomain(entity, user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<int> CountByRoleAsync(List<UserRole> roles, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => roles.Contains(u.Role))
            .CountAsync(cancellationToken);
    }
}