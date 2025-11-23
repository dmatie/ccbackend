using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class AccessRequestRepository : IAccessRequestRepository
{
    private readonly ClientConnectionDbContext _context;
    private readonly IMapper _mapper;

    public AccessRequestRepository(ClientConnectionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AccessRequest?> GetByIdAsync(Guid id)
    {
        var entity = await _context.AccessRequests
            .Include(ar => ar.ProcessedBy)
            .Include(ar => ar.Function)
            .Include(ar => ar.Country)
            .Include(ar => ar.BusinessProfile)
            .Include(ar => ar.FinancingType)
            .Include(ar => ar.Projects)
            .FirstOrDefaultAsync(ar => ar.Id == id);

        return entity == null ? null : DomainMappings.MapToDomain(entity);
    }

    public async Task<AccessRequest?> GetByEmailAsync(string email)
    {
        var entity = await _context.AccessRequests
            .Include(ar => ar.ProcessedBy)
            .Include(ar => ar.Function)
            .Include(ar => ar.Country)
            .Include(ar => ar.BusinessProfile)
            .Include(ar => ar.FinancingType)
            .Include(ar => ar.Projects)
            .FirstOrDefaultAsync(ar => ar.Email.ToLower() == email.ToLower());

        return entity == null ? null : DomainMappings.MapToDomain(entity);
    }

    public async Task<AccessRequest?> GetByEmailAndStatusAsync(string email, RequestStatus status)
    {
        var entity = await _context.AccessRequests
            .Include(ar => ar.ProcessedBy)
            .Include(ar => ar.Function)
            .Include(ar => ar.Country)
            .Include(ar => ar.BusinessProfile)
            .Include(ar => ar.FinancingType)
            .Include(ar => ar.Projects)
            .FirstOrDefaultAsync(ar => ar.Email.ToLower() == email.ToLower() && ar.Status == status);

        return entity == null ? null : DomainMappings.MapToDomain(entity);
    }


    public async Task<IEnumerable<AccessRequest>> GetAllAsync(UserContext userContext)
    {


        var query = _context.AccessRequests
            .Include(ar => ar.ProcessedBy)
            .Include(ar => ar.Function)
            .Include(ar => ar.Country)
            .Include(ar => ar.BusinessProfile)
            .Include(ar => ar.FinancingType)
            .Include(ar => ar.Projects)
            .AsQueryable();

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(c => c.CountryEntityId != null && userContext.CountryIds.Contains(c.CountryEntityId.Value));
        }

        var entities = await query
            .OrderByDescending(ar => ar.CreatedAt)
            .ToListAsync();

        return entities.Select(DomainMappings.MapToDomain);
    }

    public async Task<IEnumerable<AccessRequest>> GetByStatusAsync(UserContext userContext, RequestStatus status)
    {
        var query = _context.AccessRequests
            .Include(ar => ar.ProcessedBy)
            .Include(ar => ar.Function)
            .Include(ar => ar.Country)
            .Include(ar => ar.BusinessProfile)
            .Include(ar => ar.FinancingType)
            .Include(ar => ar.Projects)
            .Where(ar => ar.Status == status)
            .AsQueryable();

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(c => c.CountryEntityId != null && userContext.CountryIds.Contains(c.CountryEntityId.Value));
        }
        var entities = await query
            .OrderByDescending(ar => ar.CreatedAt)
            .ToListAsync();

        return entities.Select(DomainMappings.MapToDomain);
    }

    public async Task<IEnumerable<AccessRequest>> GetPendingRequestsAsync(UserContext userContext)
    {
        return await GetByStatusAsync(userContext, RequestStatus.Pending);
    }

    public async Task<AccessRequest> AddAsync(AccessRequest accessRequest)
    {
        var entity = EntityMappings.MapToEntity(accessRequest);

        entity.DomainEvents = accessRequest.DomainEvents.ToList();

        _context.AccessRequests.Add(entity);
        await _context.SaveChangesAsync();

        // Charger l'entité avec les relations pour mettre à jour l'événement
        var entityWithRelations = await _context.AccessRequests
            .Include(ar => ar.Function)
            .Include(ar => ar.Country)
            .Include(ar => ar.BusinessProfile)
            .Include(ar => ar.FinancingType)
            .Include(ar => ar.Projects)
            .FirstOrDefaultAsync(ar => ar.Id == entity.Id);

        if (entityWithRelations != null)
        {
            var domainEntity = DomainMappings.MapToDomain(entityWithRelations);
            return domainEntity;
        }

        return DomainMappings.MapToDomain(entity);
    }

    public async Task UpdateAsync(AccessRequest accessRequest)
    {
        var entity = await _context.AccessRequests.Include(ar => ar.Projects)
            .FirstOrDefaultAsync(ar => ar.Id == accessRequest.Id);

        if (entity != null)
        {
            entity.Projects.Clear();
            EntityMappings.UpdateEntityFromDomain(entity, accessRequest);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.AccessRequests.AnyAsync(ar => ar.Id == id);
    }

    public async Task<bool> ExistsEmailAsync(string email)
    {
        return await _context.AccessRequests.AnyAsync(ar => ar.Email == email);
    }

    public async Task<bool> EmailHasPendingRequestAsync(string email)
    {
        return await _context.AccessRequests
            .AnyAsync(ar => ar.Email.ToLower() == email.ToLower() && ar.Status == RequestStatus.Pending);
    }

    public async Task<bool> EmailHasStatusRequestAsync(string email, RequestStatus status)
    {
        return await _context.AccessRequests
            .AnyAsync(ar => ar.Email.ToLower() == email.ToLower() && ar.Status == status);
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await action();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> CountByStatusAsync(RequestStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.AccessRequests
            .Where(ar => ar.Status == status)
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountByStatusAsync(UserContext userContext, RequestStatus status, CancellationToken cancellationToken = default)
    {
        var query = _context.AccessRequests
            .Where(ar => ar.Status == status)
            .AsQueryable();
        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(c => c.CountryEntityId != null && userContext.CountryIds.Contains(c.CountryEntityId.Value));
        }
        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountProjectsByUserIdAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.AccessRequestProject
            .Where(arp => arp.AccessRequest.Email == email)
            .CountAsync(cancellationToken);
    }
}