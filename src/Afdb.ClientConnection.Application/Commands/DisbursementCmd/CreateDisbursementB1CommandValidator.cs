using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class CreateDisbursementB1CommandValidator : AbstractValidator<CreateDisbursementB1Command>
{
    private readonly IInputSanitizationService _sanitizationService;

    public CreateDisbursementB1CommandValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        RuleFor(x => x.GuaranteeDetails)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.GuaranteeDetailsRequired")
            .MaximumLength(1000)
            .WithMessage("ERR.Disbursement.B1.GuaranteeDetailsTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.ConfirmingBank)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ConfirmingBankRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.B1.ConfirmingBankTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.IssuingBankName)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.IssuingBankNameRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.B1.IssuingBankNameTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.IssuingBankAdress)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.IssuingBankAdressRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.B1.IssuingBankAdressTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.GuaranteeAmount)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.B1.GuaranteeAmountMustBePositive");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExpiryDateRequired")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("ERR.Disbursement.B1.ExpiryDateMustBeFuture");

        RuleFor(x => x.BeneficiaryName)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.BeneficiaryNameRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.B1.BeneficiaryNameTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.BeneficiaryBPNumber)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.BeneficiaryBPNumberRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.B1.BeneficiaryBPNumberTooLong")
            .AlphanumericWithSpaces(allowDashes: true);

        RuleFor(x => x.BeneficiaryAFDBContract)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.BeneficiaryAFDBContractRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.B1.BeneficiaryAFDBContractTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x.BeneficiaryBankAddress)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.BeneficiaryBankAddressRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.B1.BeneficiaryBankAddressTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.BeneficiaryCity)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.BeneficiaryCityRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.B1.BeneficiaryCityTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.BeneficiaryCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.BeneficiaryCountryIdRequired");

        RuleFor(x => x.GoodDescription)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.GoodDescriptionRequired")
            .MaximumLength(1000)
            .WithMessage("ERR.Disbursement.B1.GoodDescriptionTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.BeneficiaryLcContractRef)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.BeneficiaryLcContractRefRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.B1.BeneficiaryLcContractRefTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x.ExecutingAgencyName)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyNameRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyNameTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.ExecutingAgencyContactPerson)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyContactPersonRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyContactPersonTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.ExecutingAgencyAddress)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyAddressRequired")
            .MaximumLength(500)
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyAddressTooLong")
            .SafeDescription(_sanitizationService);

        RuleFor(x => x.ExecutingAgencyCity)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyCityRequired")
            .MaximumLength(100)
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyCityTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.ExecutingAgencyCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyCountryIdRequired");

        RuleFor(x => x.ExecutingAgencyEmail)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyEmailRequired")
            .EmailAddress()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyEmailInvalid")
            .MaximumLength(255)
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyEmailTooLong");

        RuleFor(x => x.ExecutingAgencyPhone)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyPhoneRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyPhoneTooLong")
            .Matches(@"^[\d\s\+\-\(\)]+$")
            .WithMessage("ERR.Disbursement.B1.ExecutingAgencyPhoneInvalid");
    }
}
