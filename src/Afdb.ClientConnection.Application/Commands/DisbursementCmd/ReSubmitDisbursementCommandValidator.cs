using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class ReSubmitDisbursementCommandValidator : AbstractValidator<ReSubmitDisbursementCommand>
{
    public ReSubmitDisbursementCommandValidator()
    {
        RuleFor(x => x.DisbursementId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.IdRequired");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.CommentRequired")
            .MaximumLength(1000)
            .WithMessage("ERR.Disbursement.CommentTooLong");
    }
}
