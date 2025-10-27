using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

internal sealed class DisbursementRepository : IDisbursementRepository
{
    private readonly ClientConnectionDbContext _context;
    private readonly ILogger<DisbursementRepository> _logger;


    public DisbursementRepository(ClientConnectionDbContext context,
        ILogger<DisbursementRepository> logger)
    {
        _context = context;
        _logger = logger;
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
            .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
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
            .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
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
            .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
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
            .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
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
            .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
            .Include(d => d.Documents)
            .FirstOrDefaultAsync(d => d.Id == entity.Id, cancellationToken);

        return entityWithRelations != null
            ? DomainMappings.MapDisbursementToDomain(entityWithRelations)
            : DomainMappings.MapDisbursementToDomain(entity);
    }

    public async Task<Disbursement> UpdateAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var entity = await _context.Disbursements
                        .Include(d => d.Processes)
                        .Include(d => d.Documents)
                        .Include(d => d.DisbursementA1)
                        .Include(d => d.DisbursementA2)
                        .Include(d => d.DisbursementA3)
                        .Include(d => d.DisbursementB1)
                        .AsTracking()
                        .FirstOrDefaultAsync(d => d.Id == disbursement.Id, cancellationToken);

            if (entity != null)
            {
                entity.DomainEvents = disbursement.DomainEvents.ToList();

                if (entity.DisbursementA1 != null)
                {
                    _context.DisbursementA1.Remove(entity.DisbursementA1);
                    entity.DisbursementA1 = null;
                }

                if (entity.DisbursementA2 != null)
                {
                    _context.DisbursementA2.Remove(entity.DisbursementA2);
                    entity.DisbursementA2 = null;
                }

                if (entity.DisbursementA3 != null)
                {
                    _context.DisbursementA3.Remove(entity.DisbursementA3);
                    entity.DisbursementA3 = null;
                }

                if (entity.DisbursementB1 != null)
                {
                    _context.DisbursementB1.Remove(entity.DisbursementB1);
                    entity.DisbursementB1 = null;
                }

                await _context.SaveChangesAsync(cancellationToken);


                EntityMappings.UpdateDisbursementEntityFromDomain(entity, disbursement);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation(
                "Disbursement {DisbursementId} updated with related entities",
                disbursement.Id);

                var entityWithRelations = await _context.Disbursements
                    .Include(d => d.DisbursementType)
                    .Include(d => d.CreatedByUser)
                    .Include(d => d.ProcessedByUser)
                    .Include(d => d.DisbursementA1)
                    .Include(d => d.DisbursementA2)
                    .Include(d => d.DisbursementA3)
                    .Include(d => d.DisbursementB1)
                    .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
                    .Include(d => d.Documents)
                    .FirstOrDefaultAsync(d => d.Id == entity.Id, cancellationToken);

                return entityWithRelations != null
                    ? DomainMappings.MapDisbursementToDomain(entityWithRelations)
                    : DomainMappings.MapDisbursementToDomain(entity);
            }

            return disbursement;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(
                ex,
                "Error updating Disbursement {DisbursementId} with related entities",
                disbursement.Id);
            throw;
        }
    }

    public async Task<Disbursement> UpdateProcessAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
                    .Include(d => d.Processes)
                    .Include(d => d.Documents)
                    .Include(d => d.DisbursementA1)
                    .Include(d => d.DisbursementA2)
                    .Include(d => d.DisbursementA3)
                    .Include(d => d.DisbursementB1)
                    .AsTracking()
                    .FirstOrDefaultAsync(d => d.Id == disbursement.Id, cancellationToken);

        if (entity != null)
        {
            entity.DomainEvents = disbursement.DomainEvents.ToList();

            EntityMappings.UpdateDisbursementProcessEntityFromDomain(entity, disbursement);
            await _context.SaveChangesAsync(cancellationToken);

            var entityWithRelations = await _context.Disbursements
                .Include(d => d.DisbursementType)
                .Include(d => d.CreatedByUser)
                .Include(d => d.ProcessedByUser)
                .Include(d => d.DisbursementA1)
                .Include(d => d.DisbursementA2)
                .Include(d => d.DisbursementA3)
                .Include(d => d.DisbursementB1)
                .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
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

        // Utilisation d'une transaction pour garantir l'unicité
        using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);

        var numbers = await _context.Disbursements
            .Where(d => d.RequestNumber.StartsWith(prefix))
            .Select(d => d.RequestNumber.Replace(prefix, ""))
            .ToListAsync(cancellationToken);

        int lastNumber = 0;
        foreach (var num in numbers)
        {
            if (int.TryParse(num, out var n) && n > lastNumber)
                lastNumber = n;
        }

        var newRequestNumber = $"{prefix}{lastNumber + 1}";

        // Vérification qu'il n'existe pas déjà (très rare mais possible en cas de course)
        var exists = await _context.Disbursements.AnyAsync(d => d.RequestNumber == newRequestNumber, cancellationToken);
        if (exists)
        {
            // Relancer la génération (récursif ou boucle)
            return await GenerateRequestNumberAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        return newRequestNumber;
    }
}
