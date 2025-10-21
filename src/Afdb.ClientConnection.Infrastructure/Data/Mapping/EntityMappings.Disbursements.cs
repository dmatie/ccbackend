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
                BeneficiaryName = disbursement.DisbursementA1.BeneficiaryName,
                BeneficiaryAddress = disbursement.DisbursementA1.BeneficiaryAddress,
                CorrespondentBankName = disbursement.DisbursementA1.BeneficiaryBankName,
                CorrespondentBankAddress = disbursement.DisbursementA1.BeneficiaryBankAddress,
                CorrespondantAccountNumber = disbursement.DisbursementA1.BeneficiaryAccountNumber,
                CorrespondentBankSwiftCode = disbursement.DisbursementA1.BeneficiarySwiftCode,
                Amount = disbursement.DisbursementA1.Amount.Amount,
                CurrencyCode = disbursement.DisbursementA1.Amount.Currency,
                IntermediaryBankName = disbursement.DisbursementA1.IntermediaryBankName,
                IntermediaryBankSwiftCode = disbursement.DisbursementA1.IntermediaryBankSwiftCode,
                SpecialInstructions = disbursement.DisbursementA1.SpecialInstructions,
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
                GoodDescription = disbursement.DisbursementA2.ClaimantName,
                ClaimantAddress = disbursement.DisbursementA2.ClaimantAddress,
                PaymentEvidenceOfPayment = disbursement.DisbursementA2.ClaimantBankName,
                ClaimantBankAddress = disbursement.DisbursementA2.ClaimantBankAddress,
                ClaimantAccountNumber = disbursement.DisbursementA2.ClaimantAccountNumber,
                ClaimantSwiftCode = disbursement.DisbursementA2.ClaimantSwiftCode,
                Amount = disbursement.DisbursementA2.Amount.Amount,
                CurrencyCode = disbursement.DisbursementA2.Amount.Currency,
                ExpenseDate = disbursement.DisbursementA2.ExpenseDate,
                ExpenseDescription = disbursement.DisbursementA2.ExpenseDescription,
                SupportingDocuments = disbursement.DisbursementA2.SupportingDocuments,
                SpecialInstructions = disbursement.DisbursementA2.SpecialInstructions,
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
                AdvancePurpose = disbursement.DisbursementA3.AdvancePurpose,
                PeriodForUtilization = disbursement.DisbursementA3.RecipientName,
                RecipientAddress = disbursement.DisbursementA3.RecipientAddress,
                GoodDescription = disbursement.DisbursementA3.RecipientBankName,
                RecipientBankAddress = disbursement.DisbursementA3.RecipientBankAddress,
                RecipientAccountNumber = disbursement.DisbursementA3.RecipientAccountNumber,
                RecipientSwiftCode = disbursement.DisbursementA3.RecipientSwiftCode,
                Amount = disbursement.DisbursementA3.Amount.Amount,
                CurrencyCode = disbursement.DisbursementA3.Amount.Currency,
                ExpectedUsageDate = disbursement.DisbursementA3.ExpectedUsageDate,
                JustificationForAdvance = disbursement.DisbursementA3.JustificationForAdvance,
                RepaymentTerms = disbursement.DisbursementA3.RepaymentTerms,
                SpecialInstructions = disbursement.DisbursementA3.SpecialInstructions,
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
                GuaranteeDetails = disbursement.DisbursementB1.GuaranteePurpose,
                IssuingBankName = disbursement.DisbursementB1.BeneficiaryName,
                IssuingBankAdress = disbursement.DisbursementB1.BeneficiaryAddress,
                BeneficiaryBankName = disbursement.DisbursementB1.BeneficiaryBankName,
                BeneficiaryBankAddress = disbursement.DisbursementB1.BeneficiaryBankAddress,
                GuaranteeAmount = disbursement.DisbursementB1.GuaranteeAmount.Amount,
                GoodDescription = disbursement.DisbursementB1.GuaranteeAmount.Currency,
                ValidityStartDate = disbursement.DisbursementB1.ValidityStartDate,
                ValidityEndDate = disbursement.DisbursementB1.ValidityEndDate,
                GuaranteeTermsAndConditions = disbursement.DisbursementB1.GuaranteeTermsAndConditions,
                SpecialInstructions = disbursement.DisbursementB1.SpecialInstructions,
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
                FileUrl = d.DocumentUrl,
                ContentType = d.ContentType,
                FileSize = d.FileSize,
                UploadedAt = d.UploadedAt,
                CreatedAt = d.CreatedAt,
                CreatedBy = d.CreatedBy,
                UpdatedAt = d.UpdatedAt,
                UpdatedBy = d.UpdatedBy
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
