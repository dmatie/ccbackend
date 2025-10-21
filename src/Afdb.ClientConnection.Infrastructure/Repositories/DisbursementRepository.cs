using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class DisbursementRepository : IDisbursementRepository
{
    private readonly ClientConnectionDbContext _context;

    public DisbursementRepository(ClientConnectionDbContext context)
    {
        _context = context;
    }

    public async Task<Disbursement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.CreatedByUser)
            .Include(d => d.ProcessedByUser)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.Processes).ThenInclude(p => p.CreatedByUser)
            .Include(d => d.Documents)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        return entity == null ? null : DomainMappings.MapDisbursementToDomain(entity);
    }

    public async Task<Disbursement?> GetByRequestNumberAsync(string requestNumber, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.CreatedByUser)
            .Include(d => d.ProcessedByUser)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.Processes).ThenInclude(p => p.CreatedByUser)
            .Include(d => d.Documents)
            .FirstOrDefaultAsync(d => d.RequestNumber == requestNumber, cancellationToken);

        return entity == null ? null : DomainMappings.MapDisbursementToDomain(entity);
    }

    public async Task<IEnumerable<Disbursement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.CreatedByUser)
            .Include(d => d.ProcessedByUser)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.Processes).ThenInclude(p => p.CreatedByUser)
            .Include(d => d.Documents)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapDisbursementToDomain).ToList();
    }

    public async Task<IEnumerable<Disbursement>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.CreatedByUser)
            .Include(d => d.ProcessedByUser)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.Processes).ThenInclude(p => p.CreatedByUser)
            .Include(d => d.Documents)
            .Where(d => d.CreatedByUserId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapDisbursementToDomain).ToList();
    }

    public async Task<Disbursement> AddAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = EntityMappings.MapDisbursementToEntity(disbursement);
        entity.DomainEvents = disbursement.DomainEvents.ToList();

        _context.Disbursements.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var entityWithRelations = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.CreatedByUser)
            .Include(d => d.ProcessedByUser)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.Processes).ThenInclude(p => p.CreatedByUser)
            .Include(d => d.Documents)
            .FirstOrDefaultAsync(d => d.Id == entity.Id, cancellationToken);

        return entityWithRelations != null
            ? DomainMappings.MapDisbursementToDomain(entityWithRelations)
            : DomainMappings.MapDisbursementToDomain(entity);
    }

    public async Task<Disbursement> UpdateAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.Processes)
            .Include(d => d.Documents)
            .AsTracking()
            .FirstOrDefaultAsync(d => d.Id == disbursement.Id, cancellationToken);

        if (entity != null)
        {
            entity.DomainEvents = disbursement.DomainEvents.ToList();
            EntityMappings.UpdateDisbursementEntityFromDomain(entity, disbursement);
            await _context.SaveChangesAsync(cancellationToken);

            var entityWithRelations = await _context.Disbursements
                .Include(d => d.DisbursementType)
                .Include(d => d.CreatedByUser)
                .Include(d => d.ProcessedByUser)
                .Include(d => d.DisbursementA1)
                .Include(d => d.DisbursementA2)
                .Include(d => d.DisbursementA3)
                .Include(d => d.DisbursementB1)
                .Include(d => d.Processes).ThenInclude(p => p.CreatedByUser)
                .Include(d => d.Documents)
                .FirstOrDefaultAsync(d => d.Id == entity.Id, cancellationToken);

            return entityWithRelations != null
                ? DomainMappings.MapDisbursementToDomain(entityWithRelations)
                : DomainMappings.MapDisbursementToDomain(entity);
        }

        return disbursement;
    }

    public async Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default)
    {
        var currentYear = DateTime.UtcNow.Year;
        var prefix = $"DIS-{currentYear}-";

        var lastDisbursement = await _context.Disbursements
            .Where(d => d.RequestNumber.StartsWith(prefix))
            .OrderByDescending(d => d.RequestNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastDisbursement == null)
        {
            return $"{prefix}1";
        }

        var lastNumberPart = lastDisbursement.RequestNumber.Replace(prefix, "");
        if (int.TryParse(lastNumberPart, out var lastNumber))
        {
            return $"{prefix}{lastNumber + 1}";
        }

        return $"{prefix}1";
    }
}
