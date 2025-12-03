using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;
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
           .Include(d => d.Currency)
           .Include(d => d.CreatedByUser)
           .Include(d => d.ProcessedByUser)
           .Include(d => d.DisbursementA1).ThenInclude(a1 => a1.BeneficiaryCountry)
           .Include(d => d.DisbursementA1).ThenInclude(a1 => a1.CorrespondentBankCountry)
           .Include(d => d.DisbursementA1).ThenInclude(a1 => a1.SignatoryCountry)
           .Include(d => d.DisbursementA2).ThenInclude(a2 => a2.GoodOrginCountry)
           .Include(d => d.DisbursementA3).ThenInclude(a3 => a3.GoodOrginCountry)
           .Include(d => d.DisbursementB1).ThenInclude(b1 => b1.BeneficiaryCountry)
           .Include(d => d.DisbursementB1).ThenInclude(b1 => b1.ExecutingAgencyCountry)
           .Include(d => d.Processes.OrderBy(c => c.CreatedAt)).ThenInclude(p => p.CreatedByUser)
           .Include(d => d.Documents)
           .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        return entity != null ? DomainMappings.MapDisbursementToDomain(entity) : null;
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

        return entity != null ? DomainMappings.MapDisbursementToDomain(entity) : null;
    }

    public async Task<IEnumerable<Disbursement>> GetAllAsync(UserContext userContext, CancellationToken cancellationToken = default)
    {
        var query = _context.Disbursements
            .Include(d => d.DisbursementType)
            .Include(d => d.Currency)
            .Include(d => d.CreatedByUser)
            .Include(d => d.ProcessedByUser)
            .AsQueryable();

        if (userContext.RequiresCountryFilter)
        {
            query = query
                .Join(
                    _context.AccessRequests,
                    disbursement => disbursement.CreatedByUser.Email,
                    accessRequest => accessRequest.Email,
                    (disbursement, accessRequest) => new { disbursement, accessRequest }
                )
                .Where(x => x.accessRequest.CountryEntityId != null &&
                    userContext.CountryIds.Contains(x.accessRequest.CountryEntityId.Value))
                .Select(x => x.disbursement);
        }

        query= query.Where(d => d.Status != DisbursementStatus.Draft);

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
            .Include(d => d.CreatedByUser)
            .Where(d => d.CreatedByUserId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapDisbursementToDomain);
    }

    public async Task<IEnumerable<Disbursement>> GetByUserIdWithPermissionsAsync(
    Guid userId,
    List<Guid> authorizedBusinessProfileIds,
    CancellationToken cancellationToken = default)
    {

        var query = _context.Disbursements
           .Include(d => d.DisbursementType)
           .Include(d => d.Currency)
           .Include(d => d.CreatedByUser)
           .Include(d => d.ProcessedByUser)
           .AsQueryable();

        query = query
            .Join(
                _context.AccessRequests,
                disbursement => disbursement.CreatedByUser.Email,
                accessRequest => accessRequest.Email,
                (disbursement, accessRequest) => new { disbursement, accessRequest }
            )
            .Where(x => x.accessRequest.BusinessProfileEntityId != null &&
                authorizedBusinessProfileIds.Contains(x.accessRequest.BusinessProfileEntityId.Value))
            .Select(x => x.disbursement);

        query = query.Where(d => d.CreatedByUserId == userId);

        var entities = await query
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(DomainMappings.MapDisbursementToDomain);

    }

    public async Task<bool> IdExist(Guid id, CancellationToken cancellationToken = default) => 
        await _context.Disbursements.AnyAsync(d => d.Id == id, cancellationToken);

    public async Task<bool> ReferenceExist(string reference, CancellationToken cancellationToken = default) => 
        await _context.Disbursements.AnyAsync(d => d.RequestNumber == reference, cancellationToken);

    public async Task<bool> FileNameExist(string reference, string fileName, CancellationToken cancellationToken = default) => 
        await _context.DisbursementDocuments
        .AnyAsync(d => d.FileName == fileName && d.Disbursement.RequestNumber == reference, cancellationToken);

    public async Task<Disbursement> AddAsync(Disbursement disbursement, CancellationToken cancellationToken = default)
    {
        var entity = EntityMappings.MapDisbursementToEntity(disbursement);
        await _context.Disbursements.AddAsync(entity, cancellationToken);
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

    public async Task<int> CountByStatusAsync(UserContext userContext, DisbursementStatus status,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Disbursements
            .Where(d => d.Status == status)
            .AsQueryable();
        if (userContext.RequiresCountryFilter)
        {
            query = query
                .Join(
                    _context.AccessRequests,
                    disbursement => disbursement.CreatedByUser.Email,
                    accessRequest => accessRequest.Email,
                    (disbursement, accessRequest) => new { disbursement, accessRequest }
                )
                .Where(x => x.accessRequest.CountryEntityId != null &&
                    userContext.CountryIds.Contains(x.accessRequest.CountryEntityId.Value))
                .Select(x => x.disbursement);
        }
        return await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountByUserIdAndStatusAsync(Guid userId, DisbursementStatus[] status, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Disbursements
            .Where(d => d.CreatedByUserId == userId && status.Contains(d.Status))
            .CountAsync(cancellationToken);
    }
}
