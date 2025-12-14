using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class OtherDocumentRepository : IOtherDocumentRepository
{
    private readonly ClientConnectionDbContext _context;
    private readonly IMapper _mapper;

    public OtherDocumentRepository(ClientConnectionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public async Task<PaginatedOtherDocumentDto> GetDocumentsFilteredAsync(
        UserContext userContext,
        OtherDocumentStatus? status,
        Guid? otherDocumentTypeId,
        string? searchTerm,
        string? year,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        if (otherDocumentTypeId.HasValue)
        {
            query = query.Where(o => o.OtherDocumentTypeId == otherDocumentTypeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var normalizedSearchTerm = searchTerm.ToLower();
            query = query.Where(o =>
                o.Name.ToLower().Contains(normalizedSearchTerm) ||
                o.SAPCode.ToLower().Contains(normalizedSearchTerm) ||
                o.LoanNumber.ToLower().Contains(normalizedSearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(year))
        {
            query = query.Where(o => o.Year == year);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var otherDocuments = entities.Select(e =>
        {
            var domain = DomainMappings.MapOtherDocumentToDomain(e);
            return _mapper.Map<OtherDocumentDto>(domain);
        }).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedOtherDocumentDto
        {
            OtherDocuments = otherDocuments,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }

    public async Task<PaginatedOtherDocumentDto> GetByUserFilteredAsync(
        Guid userId,
        OtherDocumentStatus? status,
        Guid? otherDocumentTypeId,
        string? searchTerm,
        string? year,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OtherDocuments
            .Include(o => o.OtherDocumentType)
            .Include(o => o.User)
            .Include(o => o.Files)
            .Where(o => o.UserId == userId)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        if (otherDocumentTypeId.HasValue)
        {
            query = query.Where(o => o.OtherDocumentTypeId == otherDocumentTypeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var normalizedSearchTerm = searchTerm.ToLower();
            query = query.Where(o =>
                o.Name.ToLower().Contains(normalizedSearchTerm) ||
                o.SAPCode.ToLower().Contains(normalizedSearchTerm) ||
                o.LoanNumber.ToLower().Contains(normalizedSearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(year))
        {
            query = query.Where(o => o.Year == year);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var otherDocuments = entities.Select(e =>
        {
            var domain = DomainMappings.MapOtherDocumentToDomain(e);
            return _mapper.Map<OtherDocumentDto>(domain);
        }).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedOtherDocumentDto
        {
            OtherDocuments = otherDocuments,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }
}
