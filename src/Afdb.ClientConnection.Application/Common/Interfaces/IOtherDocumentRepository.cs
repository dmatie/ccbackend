using Afdb.ClientConnection.Application.Common.Models;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IOtherDocumentRepository
{
    Task<OtherDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OtherDocument?> GetByIdWithFilesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OtherDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OtherDocument>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OtherDocument>> GetByStatusAsync(OtherDocumentStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<OtherDocument>> GetByProjectAsync(string sapCode, string loanNumber, CancellationToken cancellationToken = default);
    Task<PaginatedOtherDocumentDto> GetDocumentsFilteredAsync(
        UserContext userContext,
        OtherDocumentStatus? status,
        Guid? otherDocumentTypeId,
        string? searchTerm,
        string? year,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<PaginatedOtherDocumentDto> GetByUserFilteredAsync(
        Guid userId,
        OtherDocumentStatus? status,
        Guid? otherDocumentTypeId,
        string? searchTerm,
        string? year,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task AddAsync(OtherDocument otherDocument, CancellationToken cancellationToken = default);
    Task UpdateAsync(OtherDocument otherDocument, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
