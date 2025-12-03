using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class EditDisbursementA1CommandValidator : AbstractValidator<EditDisbursementA1Command>
{
    private readonly IInputSanitizationService _sanitizationService;

    public EditDisbursementA1CommandValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        RuleFor(x => x.PaymentPurpose)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.PaymentPurposeRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.A1.PaymentPurposeTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.BeneficiaryBpNumber)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.BeneficiaryBpNumberRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.A1.BeneficiaryBpNumberTooLong")
            .AlphanumericWithSpaces(allowDashes: true);

        RuleFor(x => x.BeneficiaryName)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.BeneficiaryNameRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A1.BeneficiaryNameTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.BeneficiaryContactPerson)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.BeneficiaryContactPersonRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A1.BeneficiaryContactPersonTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.BeneficiaryAddress)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.BeneficiaryAddressRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.A1.BeneficiaryAddressTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.BeneficiaryCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.BeneficiaryCountryIdRequired");

        RuleFor(x => x.BeneficiaryEmail)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.BeneficiaryEmailRequired")
            .EmailAddress()
            .WithMessage("ERR.Disbursement.A1.BeneficiaryEmailInvalid")
            .MaximumLength(255)
            .WithMessage("ERR.Disbursement.A1.BeneficiaryEmailTooLong");

        RuleFor(x => x.CorrespondentBankName)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankNameRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankNameTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.CorrespondentBankAddress)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankAddressRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankAddressTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.CorrespondentBankCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankCountryIdRequired");

        RuleFor(x => x.CorrespondantAccountNumber)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.CorrespondantAccountNumberRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A1.CorrespondantAccountNumberTooLong")
            .AlphanumericWithSpaces(allowDashes: true);

        RuleFor(x => x.CorrespondentBankSwiftCode)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankSwiftCodeRequired")
            .MaximumLength(11)
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankSwiftCodeTooLong")
            .Matches(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$")
            .WithMessage("ERR.Disbursement.A1.CorrespondentBankSwiftCodeInvalid");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A1.AmountMustBePositive");

        RuleFor(x => x.SignatoryName)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.SignatoryNameRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A1.SignatoryNameTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.SignatoryContactPerson)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.SignatoryContactPersonRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A1.SignatoryContactPersonTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.SignatoryAddress)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.SignatoryAddressRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.A1.SignatoryAddressTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.SignatoryCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.SignatoryCountryIdRequired");

        RuleFor(x => x.SignatoryEmail)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.SignatoryEmailRequired")
            .EmailAddress()
            .WithMessage("ERR.Disbursement.A1.SignatoryEmailInvalid")
            .MaximumLength(255)
            .WithMessage("ERR.Disbursement.A1.SignatoryEmailTooLong");

        RuleFor(x => x.SignatoryPhone)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.SignatoryPhoneRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.A1.SignatoryPhoneTooLong")
            .Matches(@"^[\d\s\+\-\(\)]+$")
            .WithMessage("ERR.Disbursement.A1.SignatoryPhoneInvalid");

        RuleFor(x => x.SignatoryTitle)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A1.SignatoryTitleRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.A1.SignatoryTitleTooLong")
            .SafeName(_sanitizationService);
    }
}
