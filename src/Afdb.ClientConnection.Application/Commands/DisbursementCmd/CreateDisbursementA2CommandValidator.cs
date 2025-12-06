using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class CreateDisbursementA2CommandValidator : AbstractValidator<CreateDisbursementA2Command?>
{
    public CreateDisbursementA2CommandValidator(IInputSanitizationService sanitizationService)
    {

        RuleFor(x => x)
           .Must(x => x != null)
           .WithMessage("ERR.Disbursement.A2.CommandCannotBeNull");

        RuleFor(x => x!.ReimbursementPurpose)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ReimbursementPurposeRequired")
            .MaximumLength(1500)
            .WithMessage("ERR.Disbursement.A2.ReimbursementPurposeTooLong")
            .SafeDescription(sanitizationService);

        RuleFor(x => x!.Contractor)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractorRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A2.ContractorTooLong")
            .SafeName(sanitizationService);

        RuleFor(x => x!.GoodDescription)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.GoodDescriptionRequired")
            .MaximumLength(1000)
            .WithMessage("ERR.Disbursement.A2.GoodDescriptionTooLong")
            .SafeDescription(sanitizationService);

        RuleFor(x => x!.GoodOrginCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.GoodOrginCountryIdRequired");

        RuleFor(x => x!.ContractBorrowerReference)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractBorrowerReferenceRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A2.ContractBorrowerReferenceTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x!.ContractAfDBReference)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractAfDBReferenceRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A2.ContractAfDBReferenceTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x!.ContractValue)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractValueRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.A2.ContractValueTooLong");

        RuleFor(x => x!.ContractBankShare)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractBankShareRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.A2.ContractBankShareTooLong");

        RuleFor(x => x!.ContractAmountPreviouslyPaid)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ERR.Disbursement.A2.ContractAmountPreviouslyPaidMustBePositive");

        RuleFor(x => x!.InvoiceRef)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.InvoiceRefRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A2.InvoiceRefTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x!.InvoiceDate)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.InvoiceDateRequired")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("ERR.Disbursement.A2.InvoiceDateCannotBeFuture");

        RuleFor(x => x!.InvoiceAmount)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A2.InvoiceAmountMustBePositive");

        RuleFor(x => x!.PaymentDateOfPayment)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.PaymentDateOfPaymentRequired")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("ERR.Disbursement.A2.PaymentDateCannotBeFuture");

        RuleFor(x => x!.PaymentAmountWithdrawn)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A2.PaymentAmountWithdrawnMustBePositive");

        RuleFor(x => x!.PaymentEvidenceOfPayment)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.PaymentEvidenceOfPaymentRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.A2.PaymentEvidenceOfPaymentTooLong")
            .SafeDescription(sanitizationService);
    }
}
