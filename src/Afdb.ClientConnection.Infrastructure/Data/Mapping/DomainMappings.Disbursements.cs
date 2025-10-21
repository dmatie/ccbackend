using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
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

        return new DisbursementA1(
            entity.Id,
            entity.DisbursementId,
            entity.PaymentPurpose,
            entity.BeneficiaryBpNumber,
            entity.BeneficiaryName,
            entity.BeneficiaryContactPerson,
            entity.BeneficiaryAddress,
            entity.BeneficiaryCountryId,
            entity.BeneficiaryEmail,
            entity.CorrespondentBankName,
            entity.CorrespondentBankAddress,
            entity.CorrespondentBankCountryId,
            entity.CorrespondantAccountNumber,
            entity.CorrespondentBankSwiftCode,
            entity.Amount,
            entity.SignatoryName,
            entity.SignatoryContactPerson,
            entity.SignatoryAddress,
            entity.SignatoryCountryId,
            entity.SignatoryEmail,
            entity.SignatoryPhone,
            entity.SignatoryTitle,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.BeneficiaryCountry != null ? MapCountry(entity.BeneficiaryCountry) : null,
            entity.CorrespondentBankCountry != null ? MapCountry(entity.CorrespondentBankCountry) : null,
            entity.SignatoryCountry != null ? MapCountry(entity.SignatoryCountry) : null
        );
    }

    public static DisbursementA2 MapDisbursementA2ToDomain(DisbursementA2Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementA2(
            entity.Id,
            entity.DisbursementId,
            entity.ReimbursementPurpose,
            entity.Contractor,
            entity.GoodDescription,
            entity.GoodOrginCountryId,
            entity.ContractBorrowerReference,
            entity.ContractAfDBReference,
            entity.ContractValue,
            entity.ContractBankShare,
            entity.ContractAmountPreviouslyPaid,
            entity.InvoiceRef,
            entity.InvoiceDate,
            entity.InvoiceAmount,
            entity.PaymentDateOfPayment,
            entity.PaymentAmountWithdrawn,
            entity.PaymentEvidenceOfPayment,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.GoodOrginCountry != null ? MapCountry(entity.GoodOrginCountry) : null
        );
    }

    public static DisbursementA3 MapDisbursementA3ToDomain(DisbursementA3Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementA3(
            entity.Id,
            entity.DisbursementId,
            entity.PeriodForUtilization,
            entity.ItemNumber,
            entity.GoodDescription,
            entity.GoodOrginCountryId,
            entity.GoodQuantity,
            entity.AnnualBudget,
            entity.BankShare,
            entity.AdvanceRequested,
            entity.DateOfApproval,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.GoodOrginCountry != null ? MapCountry(entity.GoodOrginCountry) : null
        );
    }

    public static DisbursementB1 MapDisbursementB1ToDomain(DisbursementB1Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementB1(
            entity.Id,
            entity.DisbursementId,
            entity.GuaranteeDetails,
            entity.ConfirmingBank,
            entity.IssuingBankName,
            entity.IssuingBankAdress,
            entity.GuaranteeAmount,
            entity.ExpiryDate,
            entity.BeneficiaryName,
            entity.BeneficiaryBPNumber,
            entity.BeneficiaryAFDBContract,
            entity.BeneficiaryBankAddress,
            entity.BeneficiaryCity,
            entity.BeneficiaryCountryId,
            entity.GoodDescription,
            entity.BeneficiaryLcContractRef,
            entity.ExecutingAgencyName,
            entity.ExecutingAgencyContactPerson,
            entity.ExecutingAgencyAddress,
            entity.ExecutingAgencyCity,
            entity.ExecutingAgencyCountryId,
            entity.ExecutingAgencyEmail,
            entity.ExecutingAgencyPhone,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.BeneficiaryCountry != null ? MapCountry(entity.BeneficiaryCountry) : null,
            entity.ExecutingAgencyCountry != null ? MapCountry(entity.ExecutingAgencyCountry) : null
        );
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
