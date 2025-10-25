using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Application.DTOs;

public sealed record DisbursementDto
{
    public Guid Id { get; init; }
    public string RequestNumber { get; init; } = string.Empty;
    public string SapCodeProject { get; init; } = string.Empty;
    public string LoanGrantNumber { get; init; } = string.Empty;

    public Guid CurrencyId { get; init; }
    public string Currency { get; init; } = string.Empty;
    public Guid DisbursementTypeId { get; init; }
    public string DisbursementTypeCode { get; init; } = string.Empty;
    public string DisbursementTypeName { get; init; } = string.Empty;
    public string DisbursementTypeNameFr { get; init; } = string.Empty;

    public DisbursementStatus Status { get; init; }

    public Guid CreatedByUserId { get; init; }
    public string CreatedByUserName { get; init; } = string.Empty;
    public string CreatedByUserEmail { get; init; } = string.Empty;

    public DateTime? SubmittedAt { get; init; }
    public DateTime? ProcessedAt { get; init; }

    public Guid? ProcessedByUserId { get; init; }
    public string? ProcessedByUserName { get; init; }
    public string? ProcessedByUserEmail { get; init; }

    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime? UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public DisbursementA1Dto? DisbursementA1 { get; init; }
    public DisbursementA2Dto? DisbursementA2 { get; init; }
    public DisbursementA3Dto? DisbursementA3 { get; init; }
    public DisbursementB1Dto? DisbursementB1 { get; init; }

    public List<DisbursementProcessDto> Processes { get; init; } = [];
    public List<DisbursementDocumentDto> Documents { get; init; } = [];
}
