using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class OtherDocumentRepository : IOtherDocumentRepository
{
    private readonly ClientConnectionDbContext _context;

    public OtherDocumentRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<OtherDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return entity == null ? null : DomainMappings.MapOtherDocumentToDomain(entity);
    }

    public async Task<OtherDocument?> GetByIdWithFilesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return entity == null ? null : DomainMappings.MapOtherDocumentToDomain(entity);
    }

    public async Task<IEnumerable<OtherDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapOtherDocumentToDomain).ToList();
    }

    public async Task<IEnumerable<OtherDocument>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapOtherDocumentToDomain).ToList();
    }

    public async Task<IEnumerable<OtherDocument>> GetByStatusAsync(OtherDocumentStatus status, CancellationToken cancellationToken = default)
    {
        var entities = await _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapOtherDocumentToDomain).ToList();
    }

    public async Task<IEnumerable<OtherDocument>> GetByProjectAsync(string sapCode, string loanNumber, CancellationToken cancellationToken = default)
    {
        var entities = await _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .Where(o => o.SAPCode == sapCode && o.LoanNumber == loanNumber)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapOtherDocumentToDomain).ToList();
    }

    public async Task AddAsync(OtherDocument otherDocument, CancellationToken cancellationToken = default)
    {
        var entity = EntityMappings.MapOtherDocumentToEntity(otherDocument);
        await _context.OtherDocuments.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OtherDocument otherDocument, CancellationToken cancellationToken = default)
    {
        var entity = await _context.OtherDocuments
            .Include(o => o.Files)
            .FirstOrDefaultAsync(o => o.Id == otherDocument.Id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"OtherDocument with Id {otherDocument.Id} not found");

        EntityMappings.UpdateOtherDocumentEntityFromDomain(entity, otherDocument);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.OtherDocuments
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"OtherDocument with Id {id} not found");

        _context.OtherDocuments.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IEnumerable<OtherDocument> Items, int TotalCount)> GetByUserIdWithFiltersAndPaginationAsync(
        Guid userId,
        OtherDocumentStatus? status,
        Guid? otherDocumentTypeId,
        string? sapCode,
        string? year,
        DateTime? createdFrom,
        DateTime? createdTo,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .Where(o => o.UserId == userId);

        query = ApplyFilters(query, status, otherDocumentTypeId, sapCode, year, createdFrom, createdTo);

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var domainObjects = entities.Select(DomainMappings.MapOtherDocumentToDomain).ToList();

        return (domainObjects, totalCount);
    }

    public async Task<(IEnumerable<OtherDocument> Items, int TotalCount)> GetWithFiltersAndPaginationAsync(
        UserContext userContext,
        OtherDocumentStatus? status,
        Guid? otherDocumentTypeId,
        string? sapCode,
        string? year,
        DateTime? createdFrom,
        DateTime? createdTo,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .AsQueryable();

        if (userContext.Role == UserRole.CountryAdmin)
        {
            var countryCodes = userContext.CountryCodes;
            query = query.Where(o => countryCodes.Contains(o.User.Country.Code));
        }

        query = ApplyFilters(query, status, otherDocumentTypeId, sapCode, year, createdFrom, createdTo);

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var domainObjects = entities.Select(DomainMappings.MapOtherDocumentToDomain).ToList();

        return (domainObjects, totalCount);
    }

    private static IQueryable<Data.Entities.OtherDocumentEntity> ApplyFilters(
        IQueryable<Data.Entities.OtherDocumentEntity> query,
        OtherDocumentStatus? status,
        Guid? otherDocumentTypeId,
        string? sapCode,
        string? year,
        DateTime? createdFrom,
        DateTime? createdTo)
    {
        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        if (otherDocumentTypeId.HasValue)
        {
            query = query.Where(o => o.OtherDocumentTypeId == otherDocumentTypeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(sapCode))
        {
            query = query.Where(o => o.SAPCode == sapCode);
        }

        if (!string.IsNullOrWhiteSpace(year))
        {
            query = query.Where(o => o.Year == year);
        }

        if (createdFrom.HasValue)
        {
            query = query.Where(o => o.CreatedAt >= createdFrom.Value);
        }

        if (createdTo.HasValue)
        {
            query = query.Where(o => o.CreatedAt <= createdTo.Value);
        }

        return query;
    }
}
