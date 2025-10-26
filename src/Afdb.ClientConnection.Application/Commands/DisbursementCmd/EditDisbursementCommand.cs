using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed record EditDisbursementCommand : IRequest<EditDisbursementResponse>
{
    public Guid Id { get; init; }
    public string SapCodeProject { get; init; } = string.Empty;
    public string LoanGrantNumber { get; init; } = string.Empty;
    public Guid DisbursementTypeId { get; init; }
    public Guid CurrencyId { get; set; }

    public EditDisbursementA1Command? DisbursementA1 { get; init; }
    public EditDisbursementA2Command? DisbursementA2 { get; init; }
    public EditDisbursementA3Command? DisbursementA3 { get; init; }
    public EditDisbursementB1Command? DisbursementB1 { get; init; }
    public List<IFormFile>? Documents { get; init; }
}

public sealed record EditDisbursementA1Command
{
    public string PaymentPurpose { get; init; } = string.Empty;

    public string BeneficiaryBpNumber { get; init; } = string.Empty;
    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryContactPerson { get; init; } = string.Empty;
    public string BeneficiaryAddress { get; init; } = string.Empty;
    public Guid BeneficiaryCountryId { get; init; }
    public string BeneficiaryEmail { get; init; } = string.Empty;

    public string CorrespondentBankName { get; init; } = string.Empty;
    public string CorrespondentBankAddress { get; init; } = string.Empty;
    public Guid CorrespondentBankCountryId { get; init; }
    public string CorrespondantAccountNumber { get; init; } = string.Empty;
    public string CorrespondentBankSwiftCode { get; init; } = string.Empty;

    public decimal Amount { get; init; }

    public string SignatoryName { get; init; } = string.Empty;
    public string SignatoryContactPerson { get; init; } = string.Empty;
    public string SignatoryAddress { get; init; } = string.Empty;
    public Guid SignatoryCountryId { get; init; }
    public string SignatoryEmail { get; init; } = string.Empty;
    public string SignatoryPhone { get; init; } = string.Empty;
    public string SignatoryTitle { get; init; } = string.Empty;
}

public sealed record EditDisbursementA2Command
{
    public string ReimbursementPurpose { get; init; } = string.Empty;
    public string Contractor { get; init; } = string.Empty;

    public string GoodDescription { get; init; } = string.Empty;
    public Guid GoodOrginCountryId { get; init; }

    public string ContractBorrowerReference { get; init; } = string.Empty;
    public string ContractAfDBReference { get; init; } = string.Empty;
    public string ContractValue { get; init; } = string.Empty;
    public string ContractBankShare { get; init; } = string.Empty;
    public decimal ContractAmountPreviouslyPaid { get; init; }

    public string InvoiceRef { get; init; } = string.Empty;
    public DateTime InvoiceDate { get; init; }
    public decimal InvoiceAmount { get; init; }

    public DateTime PaymentDateOfPayment { get; init; }
    public decimal PaymentAmountWithdrawn { get; init; }
    public string PaymentEvidenceOfPayment { get; init; } = string.Empty;
}

public sealed record EditDisbursementA3Command
{
    public string PeriodForUtilization { get; init; } = string.Empty;
    public int ItemNumber { get; init; }

    public string GoodDescription { get; init; } = string.Empty;
    public Guid GoodOrginCountryId { get; init; }
    public int GoodQuantity { get; init; }

    public decimal AnnualBudget { get; init; }
    public decimal BankShare { get; init; }
    public decimal AdvanceRequested { get; init; }

    public DateTime DateOfApproval { get; init; }
}

public sealed record EditDisbursementB1Command
{
    public string GuaranteeDetails { get; init; } = string.Empty;
    public string ConfirmingBank { get; init; } = string.Empty;

    public string IssuingBankName { get; init; } = string.Empty;
    public string IssuingBankAdress { get; init; } = string.Empty;
    public decimal GuaranteeAmount { get; init; }
    public DateTime ExpiryDate { get; init; }

    public string BeneficiaryName { get; init; } = string.Empty;
    public string BeneficiaryBPNumber { get; init; } = string.Empty;
    public string BeneficiaryAFDBContract { get; init; } = string.Empty;
    public string BeneficiaryBankAddress { get; init; } = string.Empty;
    public string BeneficiaryCity { get; init; } = string.Empty;
    public Guid BeneficiaryCountryId { get; init; }
    public string GoodDescription { get; init; } = string.Empty;
    public string BeneficiaryLcContractRef { get; init; } = string.Empty;

    public string ExecutingAgencyName { get; init; } = string.Empty;
    public string ExecutingAgencyContactPerson { get; init; } = string.Empty;
    public string ExecutingAgencyAddress { get; init; } = string.Empty;
    public string ExecutingAgencyCity { get; init; } = string.Empty;
    public Guid ExecutingAgencyCountryId { get; init; }
    public string ExecutingAgencyEmail { get; init; } = string.Empty;
    public string ExecutingAgencyPhone { get; init; } = string.Empty;
}

public sealed record EditDisbursementResponse
{
    public DisbursementDto Disbursement { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
