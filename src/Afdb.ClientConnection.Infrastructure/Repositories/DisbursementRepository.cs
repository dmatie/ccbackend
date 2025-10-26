using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Afdb.ClientConnection.Infrastructure.Repositories;

public sealed class DisbursementRepository : IDisbursementRepository
{
    private readonly ClientConnectionDbContext _context;
    private readonly ILogger<DisbursementRepository> _logger;

    public DisbursementRepository(
        ClientConnectionDbContext context,
        ILogger<DisbursementRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Disbursement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.DisbursementDocuments)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        return entity?.ToDomain();
    }

    public async Task<Disbursement?> GetByIdAsync(Guid id, DisbursementLoadParam loadParam)
    {
        var query = _context.Disbursements.AsQueryable();

        if (loadParam.IncludeDisbursementType)
            query = query.Include(d => d.DisbursementType);

        if (loadParam.IncludeCurrency)
            query = query.Include(d => d.Currency);

        if (loadParam.IncludeDisbursementA1)
            query = query.Include(d => d.DisbursementA1);

        if (loadParam.IncludeDisbursementA2)
            query = query.Include(d => d.DisbursementA2);

        if (loadParam.IncludeDisbursementA3)
            query = query.Include(d => d.DisbursementA3);

        if (loadParam.IncludeDisbursementB1)
            query = query.Include(d => d.DisbursementB1);

        if (loadParam.IncludeDisbursementDocuments)
            query = query.Include(d => d.DisbursementDocuments);

        var entity = await query.FirstOrDefaultAsync(d => d.Id == id);

        return entity?.ToDomain();
    }

    public async Task<Disbursement?> GetByRequestNumberAsync(string requestNumber, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.DisbursementDocuments)
            .FirstOrDefaultAsync(d => d.RequestNumber == requestNumber, cancellationToken);

        return entity?.ToDomain();
    }

    public async Task<IEnumerable<Disbursement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.CreatedByUser)
            .OrderByDescending(d => d.CreatedOn)
            .ToListAsync(cancellationToken);

        return entities.Select(e => e.ToDomain());
    }

    public async Task<IEnumerable<Disbursement>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.CreatedByUser)
            .Where(d => d.CreatedByUserId == userId)
            .OrderByDescending(d => d.CreatedOn)
            .ToListAsync(cancellationToken);

        return entities.Select(e => e.ToDomain());
    }

    public async Task<Disbursement> AddAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = disbursement.ToEntity();

        await _context.Disbursements.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var createdEntity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.DisbursementDocuments)
            .FirstAsync(d => d.Id == entity.Id, cancellationToken);

        return createdEntity.ToDomain();
    }

    public async Task<Disbursement> UpdateAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementDocuments)
            .FirstOrDefaultAsync(d => d.Id == disbursement.Id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"Disbursement with ID {disbursement.Id} not found.");

        entity.SapCodeProject = disbursement.SapCodeProject;
        entity.LoanGrantNumber = disbursement.LoanGrantNumber;
        entity.DisbursementTypeId = disbursement.DisbursementTypeId;
        entity.CurrencyId = disbursement.CurrencyId;
        entity.Status = (int)disbursement.Status;
        entity.UpdatedBy = disbursement.UpdatedBy;
        entity.UpdatedOn = disbursement.UpdatedOn;

        if (disbursement.DisbursementDocuments != null)
        {
            foreach (var doc in disbursement.DisbursementDocuments)
            {
                if (!entity.DisbursementDocuments.Any(d => d.Id == doc.Id))
                {
                    entity.DisbursementDocuments.Add(doc.ToEntity());
                }
            }
        }

        _context.Disbursements.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedEntity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .Include(d => d.DisbursementDocuments)
            .FirstAsync(d => d.Id == entity.Id, cancellationToken);

        return updatedEntity.ToDomain();
    }

    public async Task<Disbursement> UpdateWithRelatedEntitiesAsync(
        Disbursement disbursement,
        DisbursementA1? newA1,
        DisbursementA2? newA2,
        DisbursementA3? newA3,
        DisbursementB1? newB1,
        CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var entity = await _context.Disbursements
                .Include(d => d.DisbursementA1)
                .Include(d => d.DisbursementA2)
                .Include(d => d.DisbursementA3)
                .Include(d => d.DisbursementB1)
                .FirstOrDefaultAsync(d => d.Id == disbursement.Id, cancellationToken);

            if (entity == null)
                throw new InvalidOperationException($"Disbursement with ID {disbursement.Id} not found.");

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

            entity.SapCodeProject = disbursement.SapCodeProject;
            entity.LoanGrantNumber = disbursement.LoanGrantNumber;
            entity.DisbursementTypeId = disbursement.DisbursementTypeId;
            entity.CurrencyId = disbursement.CurrencyId;
            entity.Status = (int)disbursement.Status;
            entity.UpdatedBy = disbursement.UpdatedBy;
            entity.UpdatedOn = disbursement.UpdatedOn;

            if (newA1 != null)
            {
                var a1Entity = newA1.ToEntity();
                a1Entity.DisbursementId = entity.Id;
                entity.DisbursementA1 = a1Entity;
            }

            if (newA2 != null)
            {
                var a2Entity = newA2.ToEntity();
                a2Entity.DisbursementId = entity.Id;
                entity.DisbursementA2 = a2Entity;
            }

            if (newA3 != null)
            {
                var a3Entity = newA3.ToEntity();
                a3Entity.DisbursementId = entity.Id;
                entity.DisbursementA3 = a3Entity;
            }

            if (newB1 != null)
            {
                var b1Entity = newB1.ToEntity();
                b1Entity.DisbursementId = entity.Id;
                entity.DisbursementB1 = b1Entity;
            }

            _context.Disbursements.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Disbursement {DisbursementId} updated with related entities",
                disbursement.Id);

            var updatedEntity = await _context.Disbursements
                .Include(d => d.DisbursementType)
                .Include(d => d.Currency)
                .Include(d => d.DisbursementA1)
                .Include(d => d.DisbursementA2)
                .Include(d => d.DisbursementA3)
                .Include(d => d.DisbursementB1)
                .Include(d => d.DisbursementDocuments)
                .FirstAsync(d => d.Id == entity.Id, cancellationToken);

            return updatedEntity.ToDomain();
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

    public async Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var lastRequest = await _context.Disbursements
            .Where(d => d.RequestNumber.StartsWith($"DR-{year}-"))
            .OrderByDescending(d => d.RequestNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;

        if (lastRequest != null)
        {
            var parts = lastRequest.RequestNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int currentNumber))
            {
                nextNumber = currentNumber + 1;
            }
        }

        return $"DR-{year}-{nextNumber:D5}";
    }
}
