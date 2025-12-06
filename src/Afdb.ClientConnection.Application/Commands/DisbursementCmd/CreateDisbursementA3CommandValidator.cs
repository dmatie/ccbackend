using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class CreateDisbursementA3CommandValidator : AbstractValidator<CreateDisbursementA3Command?>
{
    public CreateDisbursementA3CommandValidator(IInputSanitizationService sanitizationService)
    {
        RuleFor(x => x)
           .Must(x => x != null)
           .WithMessage("ERR.Disbursement.A3.CommandCannotBeNull");


        RuleFor(x => x!.PeriodForUtilization)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A3.PeriodForUtilizationRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Disbursement.A3.PeriodForUtilizationTooLong")
            .SafeName(sanitizationService);

        RuleFor(x => x!.ItemNumber)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A3.ItemNumberMustBePositive");

        RuleFor(x => x!.GoodDescription)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A3.GoodDescriptionRequired")
            .MaximumLength(1000)
            .WithMessage("ERR.Disbursement.A3.GoodDescriptionTooLong")
            .SafeDescription(sanitizationService);

        RuleFor(x => x!.GoodOrginCountryId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A3.GoodOrginCountryIdRequired");

        RuleFor(x => x!.GoodQuantity)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A3.GoodQuantityMustBePositive");

        RuleFor(x => x!.AnnualBudget)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A3.AnnualBudgetMustBePositive");

        RuleFor(x => x!.BankShare)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A3.BankShareMustBePositive");

        RuleFor(x => x!.AdvanceRequested)
            .GreaterThan(0)
            .WithMessage("ERR.Disbursement.A3.AdvanceRequestedMustBePositive");

        RuleFor(x => x!.DateOfApproval)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.A3.DateOfApprovalRequired")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30))
            .WithMessage("ERR.Disbursement.A3.DateOfApprovalTooFarInFuture");
    }
}
