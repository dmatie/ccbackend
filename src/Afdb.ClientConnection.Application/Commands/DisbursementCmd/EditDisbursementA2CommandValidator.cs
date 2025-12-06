using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class EditDisbursementA2CommandValidator : AbstractValidator<EditDisbursementA2Command?>
{

    public EditDisbursementA2CommandValidator(IInputSanitizationService sanitizationService)
    {
        RuleFor(x => x)
           .Must(x => x != null)
           .WithMessage("ERR.Disbursement.A2.CommandCannotBeNull");

        RuleFor(x => x!.ReimbursementPurpose)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ReimbursementPurposeRequired")
            .MaximumLength(1500)
            .WithMessage("ERR.Disbursement.A2.ReimbursementPurposeMaxLength")
            .SafeDescription(sanitizationService);

        RuleFor(x => x!.Contractor)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractorRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A2.ContractorMaxLength")
            .SafeName(sanitizationService);

        RuleFor(x => x!.GoodDescription)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.GoodDescriptionRequired")
            .MaximumLength(1000)
            .WithMessage("ERR.Disbursement.A2.GoodDescriptionMaxLength")
            .SafeDescription(sanitizationService);

        RuleFor(x => x!.GoodOrginCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.GoodOrginCountryIdRequired");

        RuleFor(x => x!.ContractBorrowerReference)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractBorrowerReferenceRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A2.ContractBorrowerReferenceMaxLength")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x!.ContractAfDBReference)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractAfDBReferenceRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A2.ContractAfDBReferenceMaxLength")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x!.ContractValue)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractValueRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.A2.ContractValueMaxLength");

        RuleFor(x => x!.ContractBankShare)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.ContractBankShareRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.A2.ContractBankShareMaxLength");

        RuleFor(x => x!.ContractAmountPreviouslyPaid)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ERR.Disbursement.A2.ContractAmountPreviouslyPaidMustBePositive");

        RuleFor(x => x!.InvoiceRef)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A2.InvoiceRefRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A2.InvoiceRefMaxLength")
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
            .WithMessage("ERR.Disbursement.A2.PaymentEvidenceOfPaymentMaxLength")
            .SafeDescription(sanitizationService);
    }
}
