using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class EditDisbursementCommandValidator : AbstractValidator<EditDisbursementCommand>
{
    public EditDisbursementCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.IdRequired");

        RuleFor(x => x.SapCodeProject)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.SapCodeProjectRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.SapCodeProjectTooLong");

        RuleFor(x => x.LoanGrantNumber)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.LoanGrantNumberRequired")
            .MaximumLength(50)
            .WithMessage("ERR.Disbursement.LoanGrantNumberTooLong");

        RuleFor(x => x.DisbursementTypeId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.DisbursementTypeIdRequired");

        RuleFor(x => x.CurrencyId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.CurrencyIdRequired");

        RuleFor(x => x)
            .Must(HaveAtLeastOneFormType)
            .WithMessage("ERR.Disbursement.AtLeastOneFormTypeRequired");
    }

    private static bool HaveAtLeastOneFormType(EditDisbursementCommand command)
    {
        return command.DisbursementA1 != null ||
               command.DisbursementA2 != null ||
               command.DisbursementA3 != null ||
               command.DisbursementB1 != null;
    }
}
