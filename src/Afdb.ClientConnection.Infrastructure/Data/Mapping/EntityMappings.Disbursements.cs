using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.Graph.Models;

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
            CurrencyId = disbursement.CurrencyId,
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
            entity.DisbursementA1 = MapDisbursementA1ToEntity(disbursement.DisbursementA1);
        }

        if (disbursement.DisbursementA2 != null)
        {
            entity.DisbursementA2 = MapDisbursementA2ToEntity(disbursement.DisbursementA2);
        }

        if (disbursement.DisbursementA3 != null)
        {
            entity.DisbursementA3 = MapDisbursementA3ToEntity(disbursement.DisbursementA3);
        }

        if (disbursement.DisbursementB1 != null)
        {
            entity.DisbursementB1 = MapDisbursementB1ToEntity(disbursement.DisbursementB1);
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

    public static DisbursementA1Entity MapDisbursementA1ToEntity(DisbursementA1 disbursementA1)
    {
        return new DisbursementA1Entity
        {
            PaymentPurpose = disbursementA1.PaymentPurpose,
            BeneficiaryBpNumber = disbursementA1.BeneficiaryBpNumber,
            BeneficiaryName = disbursementA1.BeneficiaryName,
            BeneficiaryContactPerson = disbursementA1.BeneficiaryContactPerson,
            BeneficiaryAddress = disbursementA1.BeneficiaryAddress,
            BeneficiaryCountryId = disbursementA1.BeneficiaryCountryId,
            BeneficiaryEmail = disbursementA1.BeneficiaryEmail,
            CorrespondentBankName = disbursementA1.CorrespondentBankName,
            CorrespondentBankAddress = disbursementA1.CorrespondentBankAddress,
            CorrespondentBankCountryId = disbursementA1.CorrespondentBankCountryId,
            CorrespondantAccountNumber = disbursementA1.CorrespondantAccountNumber,
            CorrespondentBankSwiftCode = disbursementA1.CorrespondentBankSwiftCode,
            Amount = disbursementA1.Amount,
            SignatoryName = disbursementA1.SignatoryName,
            SignatoryContactPerson = disbursementA1.SignatoryContactPerson,
            SignatoryAddress = disbursementA1.SignatoryAddress,
            SignatoryCountryId = disbursementA1.SignatoryCountryId,
            SignatoryEmail = disbursementA1.SignatoryEmail,
            SignatoryPhone = disbursementA1.SignatoryPhone,
            SignatoryTitle = disbursementA1.SignatoryTitle,
            CreatedAt = disbursementA1.CreatedAt,
            CreatedBy = disbursementA1.CreatedBy,
            UpdatedAt = disbursementA1.UpdatedAt,
            UpdatedBy = disbursementA1.UpdatedBy
        };
    }

    public static DisbursementA2Entity MapDisbursementA2ToEntity(DisbursementA2 disbursementA2)
    {
        return new DisbursementA2Entity
        {
            ReimbursementPurpose = disbursementA2.ReimbursementPurpose,
            Contractor = disbursementA2.Contractor,
            GoodDescription = disbursementA2.GoodDescription,
            GoodOriginCountryId = disbursementA2.GoodOriginCountryId,
            ContractBorrowerReference = disbursementA2.ContractBorrowerReference,
            ContractAfDBReference = disbursementA2.ContractAfDBReference,
            ContractValue = disbursementA2.ContractValue,
            ContractBankShare = disbursementA2.ContractBankShare,
            ContractAmountPreviouslyPaid = disbursementA2.ContractAmountPreviouslyPaid,
            InvoiceRef = disbursementA2.InvoiceRef,
            InvoiceDate = disbursementA2.InvoiceDate,
            InvoiceAmount = disbursementA2.InvoiceAmount,
            PaymentDateOfPayment = disbursementA2.PaymentDateOfPayment,
            PaymentAmountWithdrawn = disbursementA2.PaymentAmountWithdrawn,
            PaymentEvidenceOfPayment = disbursementA2.PaymentEvidenceOfPayment,
            CreatedAt = disbursementA2.CreatedAt,
            CreatedBy = disbursementA2.CreatedBy,
            UpdatedAt = disbursementA2.UpdatedAt,
            UpdatedBy = disbursementA2.UpdatedBy
        };
    }

    public static DisbursementA3Entity MapDisbursementA3ToEntity(DisbursementA3 disbursementA3)
    {
        return new DisbursementA3Entity
        {
            PeriodForUtilization = disbursementA3.PeriodForUtilization,
            ItemNumber = disbursementA3.ItemNumber,
            GoodDescription = disbursementA3.GoodDescription,
            GoodOriginCountryId = disbursementA3.GoodOriginCountryId,
            GoodQuantity = disbursementA3.GoodQuantity,
            AnnualBudget = disbursementA3.AnnualBudget,
            BankShare = disbursementA3.BankShare,
            AdvanceRequested = disbursementA3.AdvanceRequested,
            DateOfApproval = disbursementA3.DateOfApproval,
            CreatedAt = disbursementA3.CreatedAt,
            CreatedBy = disbursementA3.CreatedBy,
            UpdatedAt = disbursementA3.UpdatedAt,
            UpdatedBy = disbursementA3.UpdatedBy
        };
    }


    public static DisbursementB1Entity MapDisbursementB1ToEntity(DisbursementB1 disbursementB1)
    {
        return new DisbursementB1Entity
        {
            GuaranteeDetails = disbursementB1.GuaranteeDetails,
            ConfirmingBank = disbursementB1.ConfirmingBank,
            IssuingBankName = disbursementB1.IssuingBankName,
            IssuingBankAddress = disbursementB1.IssuingBankAddress,
            GuaranteeAmount = disbursementB1.GuaranteeAmount,
            ExpiryDate = disbursementB1.ExpiryDate,
            BeneficiaryName = disbursementB1.BeneficiaryName,
            BeneficiaryBPNumber = disbursementB1.BeneficiaryBPNumber,
            BeneficiaryAFDBContract = disbursementB1.BeneficiaryAFDBContract,
            BeneficiaryBankAddress = disbursementB1.BeneficiaryBankAddress,
            BeneficiaryCity = disbursementB1.BeneficiaryCity,
            BeneficiaryCountryId = disbursementB1.BeneficiaryCountryId,
            GoodDescription = disbursementB1.GoodDescription,
            BeneficiaryLcContractRef = disbursementB1.BeneficiaryLcContractRef,
            ExecutingAgencyName = disbursementB1.ExecutingAgencyName,
            ExecutingAgencyContactPerson = disbursementB1.ExecutingAgencyContactPerson,
            ExecutingAgencyAddress = disbursementB1.ExecutingAgencyAddress,
            ExecutingAgencyCity = disbursementB1.ExecutingAgencyCity,
            ExecutingAgencyCountryId = disbursementB1.ExecutingAgencyCountryId,
            ExecutingAgencyEmail = disbursementB1.ExecutingAgencyEmail,
            ExecutingAgencyPhone = disbursementB1.ExecutingAgencyPhone,
            CreatedAt = disbursementB1.CreatedAt,
            CreatedBy = disbursementB1.CreatedBy,
            UpdatedAt = disbursementB1.UpdatedAt,
            UpdatedBy = disbursementB1.UpdatedBy
        };
    }


    public static void UpdateDisbursementEntityFromDomain(DisbursementEntity entity, Disbursement disbursement)
    {
        entity.Status = disbursement.Status;
        entity.SubmittedAt = disbursement.SubmittedAt;
        entity.ProcessedAt = disbursement.ProcessedAt;
        entity.ProcessedByUserId = disbursement.ProcessedByUserId;
        entity.UpdatedAt = disbursement.UpdatedAt;
        entity.UpdatedBy = disbursement.UpdatedBy;

        if (disbursement.DisbursementA1 != null)
        {
            entity.DisbursementA1 = MapDisbursementA1ToEntity(disbursement.DisbursementA1);
        }

        if (disbursement.DisbursementA2 != null)
        {
            entity.DisbursementA2 = MapDisbursementA2ToEntity(disbursement.DisbursementA2);
        }

        if (disbursement.DisbursementA3 != null)
        {
            entity.DisbursementA3 = MapDisbursementA3ToEntity(disbursement.DisbursementA3);
        }

        if (disbursement.DisbursementB1 != null)
        {
            entity.DisbursementB1 = MapDisbursementB1ToEntity(disbursement.DisbursementB1);
        }


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

    public static void UpdateDisbursementProcessEntityFromDomain(DisbursementEntity entity, Disbursement disbursement)
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
