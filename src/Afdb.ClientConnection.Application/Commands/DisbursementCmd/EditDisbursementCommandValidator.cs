using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class EditDisbursementCommandValidator : AbstractValidator<EditDisbursementCommand>
{
    private readonly IInputSanitizationService _sanitizationService;

    public EditDisbursementCommandValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.IdRequired");

        RuleFor(x => x.SapCodeProject)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.SapCodeProjectRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.SapCodeProjectTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x.LoanGrantNumber)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.LoanGrantNumberRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.LoanGrantNumberTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x.DisbursementTypeId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.DisbursementTypeIdRequired");

        RuleFor(x => x.CurrencyId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.CurrencyIdRequired");

        RuleFor(x => x)
            .Must(x => x.DisbursementA1 != null || x.DisbursementA2 != null ||
                      x.DisbursementA3 != null || x.DisbursementB1 != null)
            .WithMessage("ERR.Disbursement.AtLeastOneFormRequired")
            .WithName("DisbursementForms");

        RuleFor(x => x.DisbursementA1)
            .SetValidator(new EditDisbursementA1CommandValidator(_sanitizationService))
            .When(x => x.DisbursementA1 != null);

        RuleFor(x => x.DisbursementA2)
            .SetValidator(new EditDisbursementA2CommandValidator(_sanitizationService))
            .When(x => x.DisbursementA2 != null);

        RuleFor(x => x.DisbursementA3)
            .SetValidator(new EditDisbursementA3CommandValidator(_sanitizationService))
            .When(x => x.DisbursementA3 != null);

        RuleFor(x => x.DisbursementB1)
            .SetValidator(new EditDisbursementB1CommandValidator(_sanitizationService))
            .When(x => x.DisbursementB1 != null);

        RuleFor(x => x.Documents)
            .Must(docs => docs == null || docs.Count <= 10)
            .WithMessage("ERR.Disbursement.TooManyDocuments")
            .When(x => x.Documents != null);
    }
}
