using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class RejectDisbursementCommandValidator : AbstractValidator<RejectDisbursementCommand>
{
    public RejectDisbursementCommandValidator(IInputSanitizationService sanitizationService)
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.CommentEmpty")
            .MaximumLength(1500)
            .WithMessage("ERR.Disbursement.CommentMaxLength")
            .SafeDescription(sanitizationService);
    }
}
