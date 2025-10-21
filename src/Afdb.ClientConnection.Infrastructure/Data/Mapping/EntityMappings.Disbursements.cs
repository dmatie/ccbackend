using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Mapping;

internal static partial class EntityMappings
{
    public static DisbursementEntity MapDisbursementToEntity(Disbursement disbursement)
    {
        var entity = new DisbursementEntity
        {
            Id = disbursement.Id,
            RequestNumber = disbursement.RequestNumber,
            SapCodeProject = disbursement.SapCodeProject,
            LoanGrantNumber = disbursement.LoanGrantNumber,
            DisbursementTypeId = disbursement.DisbursementTypeId,
            Status = disbursement.Status,
            CreatedByUserId = disbursement.CreatedByUserId,
            SubmittedAt = disbursement.SubmittedAt,
            ProcessedAt = disbursement.ProcessedAt,
            ProcessedByUserId = disbursement.ProcessedByUserId,
            CreatedAt = disbursement.CreatedAt,
            CreatedBy = disbursement.CreatedBy,
            UpdatedAt = disbursement.UpdatedAt,
            UpdatedBy = disbursement.UpdatedBy
        };

        if (disbursement.DisbursementA1 != null)
        {
            entity.DisbursementA1 = new DisbursementA1Entity
            {
                Id = disbursement.DisbursementA1.Id,
                DisbursementId = disbursement.Id,
                PaymentPurpose = disbursement.DisbursementA1.PaymentPurpose,
                BeneficiaryBpNumber = disbursement.DisbursementA1.BeneficiaryBpNumber,
                BeneficiaryName = disbursement.DisbursementA1.BeneficiaryName,
                BeneficiaryContactPerson = disbursement.DisbursementA1.BeneficiaryContactPerson,
                BeneficiaryAddress = disbursement.DisbursementA1.BeneficiaryAddress,
                BeneficiaryCountryId = disbursement.DisbursementA1.BeneficiaryCountryId,
                BeneficiaryEmail = disbursement.DisbursementA1.BeneficiaryEmail,
                CorrespondentBankName = disbursement.DisbursementA1.CorrespondentBankName,
                CorrespondentBankAddress = disbursement.DisbursementA1.CorrespondentBankAddress,
                CorrespondentBankCountryId = disbursement.DisbursementA1.CorrespondentBankCountryId,
                CorrespondantAccountNumber = disbursement.DisbursementA1.CorrespondentAccountNumber,
                CorrespondentBankSwiftCode = disbursement.DisbursementA1.CorrespondentBankSwiftCode,
                Amount = disbursement.DisbursementA1.Amount,
                SignatoryName = disbursement.DisbursementA1.SignatoryName,
                SignatoryContactPerson = disbursement.DisbursementA1.SignatoryContactPerson,
                SignatoryAddress = disbursement.DisbursementA1.SignatoryAddress,
                SignatoryCountryId = disbursement.DisbursementA1.SignatoryCountryId,
                SignatoryEmail = disbursement.DisbursementA1.SignatoryEmail,
                SignatoryPhone = disbursement.DisbursementA1.SignatoryPhone,
                SignatoryTitle = disbursement.DisbursementA1.SignatoryTitle,
                CreatedAt = disbursement.DisbursementA1.CreatedAt,
                CreatedBy = disbursement.DisbursementA1.CreatedBy,
                UpdatedAt = disbursement.DisbursementA1.UpdatedAt,
                UpdatedBy = disbursement.DisbursementA1.UpdatedBy
            };
        }

        if (disbursement.DisbursementA2 != null)
        {
            entity.DisbursementA2 = new DisbursementA2Entity
            {
                Id = disbursement.DisbursementA2.Id,
                DisbursementId = disbursement.Id,
                ReimbursementPurpose = disbursement.DisbursementA2.ReimbursementPurpose,
                Contractor = disbursement.DisbursementA2.Contractor,
                GoodDescription = disbursement.DisbursementA2.GoodDescription,
                GoodOrginCountryId = disbursement.DisbursementA2.GoodOriginCountryId,
                ContractBorrowerReference = disbursement.DisbursementA2.ContractBorrowerReference,
                ContractAfDBReference = disbursement.DisbursementA2.ContractAfDBReference,
                ContractValue = disbursement.DisbursementA2.ContractValue,
                ContractBankShare = disbursement.DisbursementA2.ContractBankShare,
                ContractAmountPreviouslyPaid = disbursement.DisbursementA2.ContractAmountPreviouslyPaid,
                InvoiceRef = disbursement.DisbursementA2.InvoiceRef,
                InvoiceDate = disbursement.DisbursementA2.InvoiceDate,
                InvoiceAmount = disbursement.DisbursementA2.InvoiceAmount,
                PaymentDateOfPayment = disbursement.DisbursementA2.PaymentDateOfPayment,
                PaymentAmountWithdrawn = disbursement.DisbursementA2.PaymentAmountWithdrawn,
                PaymentEvidenceOfPayment = disbursement.DisbursementA2.PaymentEvidenceOfPayment,
                CreatedAt = disbursement.DisbursementA2.CreatedAt,
                CreatedBy = disbursement.DisbursementA2.CreatedBy,
                UpdatedAt = disbursement.DisbursementA2.UpdatedAt,
                UpdatedBy = disbursement.DisbursementA2.UpdatedBy
            };
        }

        if (disbursement.DisbursementA3 != null)
        {
            entity.DisbursementA3 = new DisbursementA3Entity
            {
                Id = disbursement.DisbursementA3.Id,
                DisbursementId = disbursement.Id,
                PeriodForUtilization = disbursement.DisbursementA3.PeriodForUtilization,
                ItemNumber = disbursement.DisbursementA3.ItemNumber,
                GoodDescription = disbursement.DisbursementA3.GoodDescription,
                GoodOrginCountryId = disbursement.DisbursementA3.GoodOriginCountryId,
                GoodQuantity = disbursement.DisbursementA3.GoodQuantity,
                AnnualBudget = disbursement.DisbursementA3.AnnualBudget,
                BankShare = disbursement.DisbursementA3.BankShare,
                AdvanceRequested = disbursement.DisbursementA3.AdvanceRequested,
                DateOfApproval = disbursement.DisbursementA3.DateOfApproval,
                CreatedAt = disbursement.DisbursementA3.CreatedAt,
                CreatedBy = disbursement.DisbursementA3.CreatedBy,
                UpdatedAt = disbursement.DisbursementA3.UpdatedAt,
                UpdatedBy = disbursement.DisbursementA3.UpdatedBy
            };
        }

        if (disbursement.DisbursementB1 != null)
        {
            entity.DisbursementB1 = new DisbursementB1Entity
            {
                Id = disbursement.DisbursementB1.Id,
                DisbursementId = disbursement.Id,
                GuaranteeDetails = disbursement.DisbursementB1.GuaranteeDetails,
                ConfirmingBank = disbursement.DisbursementB1.ConfirmingBank,
                IssuingBankName = disbursement.DisbursementB1.IssuingBankName,
                IssuingBankAdress = disbursement.DisbursementB1.IssuingBankAddress,
                GuaranteeAmount = disbursement.DisbursementB1.GuaranteeAmount,
                ExpiryDate = disbursement.DisbursementB1.ExpiryDate,
                BeneficiaryName = disbursement.DisbursementB1.BeneficiaryName,
                BeneficiaryBPNumber = disbursement.DisbursementB1.BeneficiaryBPNumber,
                BeneficiaryAFDBContract = disbursement.DisbursementB1.BeneficiaryAFDBContract,
                BeneficiaryBankAddress = disbursement.DisbursementB1.BeneficiaryBankAddress,
                BeneficiaryCity = disbursement.DisbursementB1.BeneficiaryCity,
                BeneficiaryCountryId = disbursement.DisbursementB1.BeneficiaryCountryId,
                GoodDescription = disbursement.DisbursementB1.GoodDescription,
                BeneficiaryLcContractRef = disbursement.DisbursementB1.BeneficiaryLcContractRef,
                ExecutingAgencyName = disbursement.DisbursementB1.ExecutingAgencyName,
                ExecutingAgencyContactPerson = disbursement.DisbursementB1.ExecutingAgencyContactPerson,
                ExecutingAgencyAddress = disbursement.DisbursementB1.ExecutingAgencyAddress,
                ExecutingAgencyCity = disbursement.DisbursementB1.ExecutingAgencyCity,
                ExecutingAgencyCountryId = disbursement.DisbursementB1.ExecutingAgencyCountryId,
                ExecutingAgencyEmail = disbursement.DisbursementB1.ExecutingAgencyEmail,
                ExecutingAgencyPhone = disbursement.DisbursementB1.ExecutingAgencyPhone,
                CreatedAt = disbursement.DisbursementB1.CreatedAt,
                CreatedBy = disbursement.DisbursementB1.CreatedBy,
                UpdatedAt = disbursement.DisbursementB1.UpdatedAt,
                UpdatedBy = disbursement.DisbursementB1.UpdatedBy
            };
        }

        if (disbursement.Processes.Count > 0)
        {
            entity.Processes = disbursement.Processes.Select(p => new DisbursementProcessEntity
            {
                Id = p.Id,
                DisbursementId = disbursement.Id,
                Status = p.Status,
                CreatedByUserId = p.CreatedByUserId,
                Comment = p.Comment,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                UpdatedAt = p.UpdatedAt,
                UpdatedBy = p.UpdatedBy
            }).ToList();
        }

        if (disbursement.Documents.Count > 0)
        {
            entity.Documents = disbursement.Documents.Select(d => new DisbursementDocumentEntity
            {
                Id = d.Id,
                DisbursementId = disbursement.Id,
                FileName = d.FileName,
                DocumentUrl = d.DocumentUrl,
                CreatedAt = d.CreatedAt,
                CreatedBy = d.CreatedBy
            }).ToList();
        }

        entity.DomainEvents = disbursement.DomainEvents.ToList();

        return entity;
    }

    public static void UpdateDisbursementEntityFromDomain(DisbursementEntity entity, Disbursement disbursement)
    {
        entity.Status = disbursement.Status;
        entity.SubmittedAt = disbursement.SubmittedAt;
        entity.ProcessedAt = disbursement.ProcessedAt;
        entity.ProcessedByUserId = disbursement.ProcessedByUserId;
        entity.UpdatedAt = disbursement.UpdatedAt;
        entity.UpdatedBy = disbursement.UpdatedBy;

        if (disbursement.Processes.Count > 0)
        {
            var existingProcessIds = entity.Processes.Select(p => p.Id).ToHashSet();
            var newProcesses = disbursement.Processes.Where(p => !existingProcessIds.Contains(p.Id));

            foreach (var process in newProcesses)
            {
                entity.Processes.Add(new DisbursementProcessEntity
                {
                    Id = process.Id,
                    DisbursementId = disbursement.Id,
                    Status = process.Status,
                    CreatedByUserId = process.CreatedByUserId,
                    Comment = process.Comment,
                    CreatedAt = process.CreatedAt,
                    CreatedBy = process.CreatedBy,
                    UpdatedAt = process.UpdatedAt,
                    UpdatedBy = process.UpdatedBy
                });
            }
        }

        entity.DomainEvents = disbursement.DomainEvents.ToList();
    }
}
