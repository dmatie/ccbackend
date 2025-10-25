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
            CurrencyId = entity.CurrencyId,
            Currency = entity.Currency != null ? MapCurrencyToDomain(entity.Currency) : null,
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
            BeneficiaryBpNumber = entity.BeneficiaryBpNumber,
            BeneficiaryName = entity.BeneficiaryName,
            BeneficiaryContactPerson = entity.BeneficiaryContactPerson,
            BeneficiaryAddress = entity.BeneficiaryAddress,
            BeneficiaryCountryId = entity.BeneficiaryCountryId,
            BeneficiaryEmail = entity.BeneficiaryEmail,
            CorrespondentBankName = entity.CorrespondentBankName,
            CorrespondentBankAddress = entity.CorrespondentBankAddress,
            CorrespondentBankCountryId = entity.CorrespondentBankCountryId,
            CorrespondantAccountNumber = entity.CorrespondantAccountNumber,
            CorrespondentBankSwiftCode = entity.CorrespondentBankSwiftCode,
            Amount = entity.Amount,
            SignatoryName = entity.SignatoryName,
            SignatoryContactPerson = entity.SignatoryContactPerson,
            SignatoryAddress = entity.SignatoryAddress,
            SignatoryCountryId = entity.SignatoryCountryId,
            SignatoryEmail = entity.SignatoryEmail,
            SignatoryPhone = entity.SignatoryPhone,
            SignatoryTitle = entity.SignatoryTitle,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
            BeneficiaryCountry = entity.BeneficiaryCountry != null ? MapCountry(entity.BeneficiaryCountry) : null,
            CorrespondentBankCountry = entity.CorrespondentBankCountry != null ? MapCountry(entity.CorrespondentBankCountry) : null,
            SignatoryCountry = entity.SignatoryCountry != null ? MapCountry(entity.SignatoryCountry) : null
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
            Contractor = entity.Contractor,
            GoodDescription = entity.GoodDescription,
            GoodOrginCountryId = entity.GoodOriginCountryId,
            ContractBorrowerReference = entity.ContractBorrowerReference,
            ContractAfDBReference = entity.ContractAfDBReference,
            ContractValue = entity.ContractValue,
            ContractBankShare = entity.ContractBankShare,
            ContractAmountPreviouslyPaid = entity.ContractAmountPreviouslyPaid,
            InvoiceRef = entity.InvoiceRef,
            InvoiceDate = entity.InvoiceDate,
            InvoiceAmount = entity.InvoiceAmount,
            PaymentDateOfPayment = entity.PaymentDateOfPayment,
            PaymentAmountWithdrawn = entity.PaymentAmountWithdrawn,
            PaymentEvidenceOfPayment = entity.PaymentEvidenceOfPayment,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
            GoodOrginCountry = entity.GoodOrginCountry != null ? MapCountry(entity.GoodOrginCountry) : null
        });
    }

    public static DisbursementA3 MapDisbursementA3ToDomain(DisbursementA3Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementA3(new DisbursementA3LoadParam
        {
            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            PeriodForUtilization = entity.PeriodForUtilization,
            ItemNumber = entity.ItemNumber,
            GoodDescription = entity.GoodDescription,
            GoodOrginCountryId = entity.GoodOriginCountryId,
            GoodQuantity = entity.GoodQuantity,
            AnnualBudget = entity.AnnualBudget,
            BankShare = entity.BankShare,
            AdvanceRequested = entity.AdvanceRequested,
            DateOfApproval = entity.DateOfApproval,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
            GoodOrginCountry = entity.GoodOrginCountry != null ? MapCountry(entity.GoodOrginCountry) : null
        });
    }

    public static DisbursementB1 MapDisbursementB1ToDomain(DisbursementB1Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new DisbursementB1(new DisbursementB1LoadParam
        {

            Id = entity.Id,
            DisbursementId = entity.DisbursementId,
            GuaranteeDetails = entity.GuaranteeDetails,
            ConfirmingBank = entity.ConfirmingBank,
            IssuingBankName = entity.IssuingBankName,
            IssuingBankAdress = entity.IssuingBankAddress,
            GuaranteeAmount = entity.GuaranteeAmount,
            ExpiryDate = entity.ExpiryDate,
            BeneficiaryName = entity.BeneficiaryName,
            BeneficiaryBPNumber = entity.BeneficiaryBPNumber,
            BeneficiaryAFDBContract = entity.BeneficiaryAFDBContract,
            BeneficiaryBankAddress = entity.BeneficiaryBankAddress,
            BeneficiaryCity = entity.BeneficiaryCity,
            BeneficiaryCountryId = entity.BeneficiaryCountryId,
            GoodDescription = entity.GoodDescription,
            BeneficiaryLcContractRef = entity.BeneficiaryLcContractRef,
            ExecutingAgencyName = entity.ExecutingAgencyName,
            ExecutingAgencyContactPerson = entity.ExecutingAgencyContactPerson,
            ExecutingAgencyAddress = entity.ExecutingAgencyAddress,
            ExecutingAgencyCity = entity.ExecutingAgencyCity,
            ExecutingAgencyCountryId = entity.ExecutingAgencyCountryId,
            ExecutingAgencyEmail = entity.ExecutingAgencyEmail,
            ExecutingAgencyPhone = entity.ExecutingAgencyPhone,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
            BeneficiaryCountry = entity.BeneficiaryCountry != null ? MapCountry(entity.BeneficiaryCountry) : null,
            ExecutingAgencyCountry = entity.ExecutingAgencyCountry != null ? MapCountry(entity.ExecutingAgencyCountry) : null
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
            Comment = entity.Comment ?? "",
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
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });
    }


    public static Currency MapCurrencyToDomain(CurrencyEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new Currency(new CurrencyLoadParam
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Symbol = entity.Symbol,
            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy
        });
    }
}
