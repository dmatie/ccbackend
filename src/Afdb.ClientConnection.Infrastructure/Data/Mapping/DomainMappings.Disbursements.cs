using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.ValueObjects;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class DomainMappings
{
    public static Disbursement MapDisbursementToDomain(DisbursementEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var disbursement = new Disbursement(new DisbursementLoadParam
        {
            Id = entity.Id,
            RequestNumber = entity.RequestNumber,
            SapCodeProject = entity.SapCodeProject,
            LoanGrantNumber = entity.LoanGrantNumber,
            DisbursementTypeId = entity.DisbursementTypeId,
            DisbursementType = entity.DisbursementType != null
                ? new DisbursementType(entity.DisbursementType.Id, entity.DisbursementType.Code, entity.DisbursementType.Name, entity.DisbursementType.NameFr, entity.DisbursementType.Description, entity.DisbursementType.CreatedAt, entity.DisbursementType.CreatedBy, entity.DisbursementType.UpdatedAt, entity.DisbursementType.UpdatedBy)
                : null,
            Status = entity.Status,
            CreatedByUserId = entity.CreatedByUserId,
            CreatedByUser = entity.CreatedByUser != null ? MapUser(entity.CreatedByUser) : null,
            SubmittedAt = entity.SubmittedAt,
            ProcessedAt = entity.ProcessedAt,
            ProcessedByUserId = entity.ProcessedByUserId,
            ProcessedByUser = entity.ProcessedByUser != null ? MapUser(entity.ProcessedByUser) : null,
            DisbursementA1 = entity.DisbursementA1 != null ? MapDisbursementA1ToDomain(entity.DisbursementA1) : null,
            DisbursementA2 = entity.DisbursementA2 != null ? MapDisbursementA2ToDomain(entity.DisbursementA2) : null,
            DisbursementA3 = entity.DisbursementA3 != null ? MapDisbursementA3ToDomain(entity.DisbursementA3) : null,
            DisbursementB1 = entity.DisbursementB1 != null ? MapDisbursementB1ToDomain(entity.DisbursementB1) : null,
            Processes = entity.Processes?.Select(MapDisbursementProcessToDomain).ToList() ?? [],
            Documents = entity.Documents?.Select(MapDisbursementDocumentToDomain).ToList() ?? [],
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });

        return disbursement;
    }

    public static DisbursementA1 MapDisbursementA1ToDomain(DisbursementA1Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementA1(new DisbursementA1LoadParam
        {
            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            PaymentPurpose = entity.PaymentPurpose,
            BeneficiaryName = entity.BeneficiaryName,
            BeneficiaryAddress = entity.BeneficiaryAddress,
            BeneficiaryBankName = entity.CorrespondentBankName,
            BeneficiaryBankAddress = entity.CorrespondentBankAddress,
            BeneficiaryAccountNumber = entity.CorrespondantAccountNumber,
            BeneficiarySwiftCode = entity.CorrespondentBankSwiftCode,
            Amount = new Money(entity.Amount, entity.CurrencyCode),
            IntermediaryBankName = entity.IntermediaryBankName,
            IntermediaryBankSwiftCode = entity.IntermediaryBankSwiftCode,
            SpecialInstructions = entity.SpecialInstructions,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });
    }

    public static DisbursementA2 MapDisbursementA2ToDomain(DisbursementA2Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementA2(new DisbursementA2LoadParam
        {
            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            ReimbursementPurpose = entity.ReimbursementPurpose,
            ClaimantName = entity.GoodDescription,
            ClaimantAddress = entity.ClaimantAddress,
            ClaimantBankName = entity.PaymentEvidenceOfPayment,
            ClaimantBankAddress = entity.ClaimantBankAddress,
            ClaimantAccountNumber = entity.ClaimantAccountNumber,
            ClaimantSwiftCode = entity.ClaimantSwiftCode,
            Amount = new Money(entity.Amount, entity.CurrencyCode),
            ExpenseDate = entity.ExpenseDate,
            ExpenseDescription = entity.ExpenseDescription,
            SupportingDocuments = entity.SupportingDocuments,
            SpecialInstructions = entity.SpecialInstructions,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });
    }

    public static DisbursementA3 MapDisbursementA3ToDomain(DisbursementA3Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementA3(new DisbursementA3LoadParam
        {
            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            AdvancePurpose = entity.AdvancePurpose,
            RecipientName = entity.PeriodForUtilization,
            RecipientAddress = entity.RecipientAddress,
            RecipientBankName = entity.GoodDescription,
            RecipientBankAddress = entity.RecipientBankAddress,
            RecipientAccountNumber = entity.RecipientAccountNumber,
            RecipientSwiftCode = entity.RecipientSwiftCode,
            Amount = new Money(entity.Amount, entity.CurrencyCode),
            ExpectedUsageDate = entity.ExpectedUsageDate,
            JustificationForAdvance = entity.JustificationForAdvance,
            RepaymentTerms = entity.RepaymentTerms,
            SpecialInstructions = entity.SpecialInstructions,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });
    }

    public static DisbursementB1 MapDisbursementB1ToDomain(DisbursementB1Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementB1(new DisbursementB1LoadParam
        {
            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            GuaranteePurpose = entity.GuaranteeDetails,
            BeneficiaryName = entity.IssuingBankName,
            BeneficiaryAddress = entity.IssuingBankAdress,
            BeneficiaryBankName = entity.BeneficiaryBankName,
            BeneficiaryBankAddress = entity.BeneficiaryBankAddress,
            GuaranteeAmount = new Money(entity.GuaranteeAmount, entity.GoodDescription),
            ValidityStartDate = entity.ValidityStartDate,
            ValidityEndDate = entity.ValidityEndDate,
            GuaranteeTermsAndConditions = entity.GuaranteeTermsAndConditions,
            SpecialInstructions = entity.SpecialInstructions,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });
    }

    public static DisbursementProcess MapDisbursementProcessToDomain(DisbursementProcessEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementProcess(new DisbursementProcessLoadParam
        {
            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            Status = entity.Status,
            CreatedByUserId = entity.CreatedByUserId,
            CreatedByUser = entity.CreatedByUser != null ? MapUser(entity.CreatedByUser) : null,
            Comment = entity.Comment??"",
            ProcessedAt = entity.CreatedAt,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });
    }

    public static DisbursementDocument MapDisbursementDocumentToDomain(DisbursementDocumentEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementDocument(new DisbursementDocumentLoadParam
        {
            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            FileName = entity.FileName,
            DocumentUrl = entity.DocumentUrl,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy
        });
    }
}
