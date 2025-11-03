using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
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
            .Include(d => d.Currency)
            .Include(d => d.User)
            .Include(d => d.DisbursementProcesses)
            .Include(d => d.DisbursementDocuments)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        return entity != null ? DomainMappings.MapDisbursementToDomain(entity) : null;
    }

    public async Task<Disbursement?> GetByRequestNumberAsync(string requestNumber, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.User)
            .Include(d => d.DisbursementProcesses)
            .Include(d => d.DisbursementDocuments)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .FirstOrDefaultAsync(d => d.RequestNumber == requestNumber, cancellationToken);

        return entity != null ? DomainMappings.MapDisbursementToDomain(entity) : null;
    }

    public async Task<IEnumerable<Disbursement>> GetAllAsync(UserContext userContext, CancellationToken cancellationToken = default)
    {
        var query = _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.User)
            .Include(d => d.DisbursementProcesses)
            .AsQueryable();

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(d => userContext.CountryIds.Contains(d.CountryId));
        }

        var entities = await query
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapDisbursementToDomain);
    }

    public async Task<IEnumerable<Disbursement>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.User)
            .Include(d => d.DisbursementProcesses)
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapDisbursementToDomain);
    }

    public async Task<Disbursement> AddAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = EntityMappings.MapDisbursementToEntity(disbursement);
        await _context.Disbursements.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var savedEntity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.User)
            .Include(d => d.DisbursementProcesses)
            .Include(d => d.DisbursementDocuments)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .FirstAsync(d => d.Id == entity.Id, cancellationToken);

        return DomainMappings.MapDisbursementToDomain(savedEntity);
    }

    public async Task<Disbursement> UpdateAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementDocuments)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .FirstOrDefaultAsync(d => d.Id == disbursement.Id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"Disbursement with ID {disbursement.Id} not found");

        EntityMappings.UpdateDisbursementEntity(entity, disbursement);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedEntity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.User)
            .Include(d => d.DisbursementProcesses)
            .Include(d => d.DisbursementDocuments)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .FirstAsync(d => d.Id == entity.Id, cancellationToken);

        return DomainMappings.MapDisbursementToDomain(updatedEntity);
    }

    public async Task<Disbursement> UpdateProcessAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Disbursements
            .Include(d => d.DisbursementProcesses)
            .FirstOrDefaultAsync(d => d.Id == disbursement.Id, cancellationToken);

        if (entity == null)
            throw new InvalidOperationException($"Disbursement with ID {disbursement.Id} not found");

        EntityMappings.UpdateDisbursementProcessEntity(entity, disbursement);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedEntity = await _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.User)
            .Include(d => d.DisbursementProcesses)
            .Include(d => d.DisbursementDocuments)
            .Include(d => d.DisbursementA1)
            .Include(d => d.DisbursementA2)
            .Include(d => d.DisbursementA3)
            .Include(d => d.DisbursementB1)
            .FirstAsync(d => d.Id == entity.Id, cancellationToken);

        return DomainMappings.MapDisbursementToDomain(updatedEntity);
    }

    public async Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"DR-{year}-";

        var lastRequestNumber = await _context.Disbursements
            .Where(d => d.RequestNumber.StartsWith(prefix))
            .OrderByDescending(d => d.RequestNumber)
            .Select(d => d.RequestNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastRequestNumber == null)
        {
            return $"{prefix}0001";
        }

        var lastNumber = int.Parse(lastRequestNumber.Substring(prefix.Length));
        var newNumber = lastNumber + 1;

        return $"{prefix}{newNumber:D4}";
    }

    public async Task<int> CountByStatusAsync(DisbursementStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Disbursements
            .Where(d => d.Status == status)
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountByStatusAsync(UserContext userContext, DisbursementStatus status, CancellationToken cancellationToken = default)
    {
        var query = _context.Disbursements
            .Where(d => d.Status == status);

        if (userContext.RequiresCountryFilter)
        {
            query = query.Where(d => userContext.CountryIds.Contains(d.CountryId));
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountByUserIdAndStatusAsync(Guid userId, DisbursementStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Disbursements
            .Where(d => d.UserId == userId && d.Status == status)
            .CountAsync(cancellationToken);
    }
}
