using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record AccessRequestDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public RequestStatus Status { get; init; }
    public DateTime RequestedDate { get; init; }
    public DateTime? ProcessedDate { get; init; }
    public Guid? ProcessedById { get; init; }
    public string? ProcessingComments { get; init; }
    public string? EntraIdObjectId { get; init; }

    // Nouvelles propriétés de liaison
    public Guid? FunctionId { get; init; }
    public Guid? CountryId { get; init; }
    public Guid? BusinessProfileId { get; init; }
    public Guid? FinancingTypeId { get; init; }

    // Noms des entités de référence
    public string? FunctionName { get; init; }
    public string? CountryName { get; init; }
    public string? BusinessProfileName { get; init; }
    public string? FinancingTypeName { get; init; }

    public string[] ApproversEmail { get; init; } = Array.Empty<string>();
    public string FullName => $"{FirstName} {LastName}";
    public bool CanBeProcessed => Status == RequestStatus.Pending;
    public bool IsProcessed => Status != RequestStatus.Pending;
    public bool HasEntraIdAccount => !string.IsNullOrWhiteSpace(EntraIdObjectId);
    public List<String> SelectedProjectCodes { get; init; } = default!;
    public string RegistrationCode { get; init; } = default!;
    public List<AccessRequestProjectDto> Projects { get; init; } = default!;
    public List<AccessRequestDocumentDto> Documents { get; init; } = new();
}

