using Afdb.ClientConnection.Application.Common.Interfaces;
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
}
