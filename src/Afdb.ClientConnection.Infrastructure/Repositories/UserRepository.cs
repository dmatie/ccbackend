using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly ClientConnectionDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(ClientConnectionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<User?> GetByEntraIdObjectIdAsync(string entraIdObjectId)
    {
        var entity = await _context.Users
            .FirstOrDefaultAsync(u => u.EntraIdObjectId == entraIdObjectId);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        var entities = await _context.Users
            .Where(u => u.IsActive)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<User>> GetActiveUsersByRolesAsync(List<UserRole> roles)
    {
        var entities = await _context.Users
            .Where(u => u.IsActive && roles.Contains(u.Role))
            .ToListAsync();

        return entities.Select(MapToDomain);
    }


    public async Task<User> AddAsync(User user)
    {
        var entity = MapToEntity(user);
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();
        return MapToDomain(entity);
    }

    public async Task UpdateAsync(User user)
    {
        var entity = await _context.Users.FindAsync(user.Id);
        if (entity != null)
        {
            UpdateEntityFromDomain(entity, user);
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

    private static User MapToDomain(UserEntity entity)
    {
        // Use reflection to create domain entity with private constructor
        var user = (User)Activator.CreateInstance(typeof(User), true)!;

        // Set properties using reflection
        typeof(User).GetProperty("Id")!.SetValue(user, entity.Id);
        typeof(User).GetProperty("Email")!.SetValue(user, entity.Email);
        typeof(User).GetProperty("FirstName")!.SetValue(user, entity.FirstName);
        typeof(User).GetProperty("LastName")!.SetValue(user, entity.LastName);
        typeof(User).GetProperty("Role")!.SetValue(user, entity.Role);
        typeof(User).GetProperty("IsActive")!.SetValue(user, entity.IsActive);
        typeof(User).GetProperty("EntraIdObjectId")!.SetValue(user, entity.EntraIdObjectId);
        typeof(User).GetProperty("OrganizationName")!.SetValue(user, entity.OrganizationName);
        typeof(User).GetProperty("CreatedAt")!.SetValue(user, entity.CreatedAt);
        typeof(User).GetProperty("CreatedBy")!.SetValue(user, entity.CreatedBy);
        typeof(User).GetProperty("UpdatedAt")!.SetValue(user, entity.UpdatedAt);
        typeof(User).GetProperty("UpdatedBy")!.SetValue(user, entity.UpdatedBy);

        return user;
    }

    private static UserEntity MapToEntity(User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            IsActive = user.IsActive,
            EntraIdObjectId = user.EntraIdObjectId,
            OrganizationName = user.OrganizationName,
            CreatedAt = user.CreatedAt,
            CreatedBy = user.CreatedBy,
            UpdatedAt = user.UpdatedAt,
            UpdatedBy = user.UpdatedBy
        };
    }

    private static void UpdateEntityFromDomain(UserEntity entity, User user)
    {
        entity.Email = user.Email;
        entity.FirstName = user.FirstName;
        entity.LastName = user.LastName;
        entity.Role = user.Role;
        entity.IsActive = user.IsActive;
        entity.EntraIdObjectId = user.EntraIdObjectId;
        entity.OrganizationName = user.OrganizationName;
        entity.UpdatedAt = user.UpdatedAt;
        entity.UpdatedBy = user.UpdatedBy;
    }
}