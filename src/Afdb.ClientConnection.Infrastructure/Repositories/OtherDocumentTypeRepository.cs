using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class OtherDocumentTypeRepository : IOtherDocumentTypeRepository
{
    private readonly ClientConnectionDbContext _context;

    public OtherDocumentTypeRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<OtherDocumentType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.OtherDocumentTypes
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return entity == null ? null : DomainMappings.MapOtherDocumentTypeToDomain(entity);
    }

    public async Task<IEnumerable<OtherDocumentType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.OtherDocumentTypes
            .Where(o => o.IsActive)
            .OrderBy(o => o.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapOtherDocumentTypeToDomain).ToList();
    }

    public async Task AddAsync(OtherDocumentType otherDocumentType, CancellationToken cancellationToken = default)
    {
        var entity = new OtherDocumentTypeEntity
        {
            Id = otherDocumentType.Id,
            Name = otherDocumentType.Name,
            NameFr = otherDocumentType.NameFr,
            IsActive = otherDocumentType.IsActive,
            CreatedAt = otherDocumentType.CreatedAt,
            CreatedBy = otherDocumentType.CreatedBy
        };
        await _context.OtherDocumentTypes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(OtherDocumentType otherDocumentType, CancellationToken cancellationToken = default)
    {
        var entity = await _context.OtherDocumentTypes
            .FirstOrDefaultAsync(o => o.Id == otherDocumentType.Id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"OtherDocumentType with Id {otherDocumentType.Id} not found");

        entity.Name = otherDocumentType.Name;
        entity.NameFr = otherDocumentType.NameFr;
        entity.IsActive = otherDocumentType.IsActive;
        entity.UpdatedAt = otherDocumentType.UpdatedAt;
        entity.UpdatedBy = otherDocumentType.UpdatedBy;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
