using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public class RejectAccessRequestCommandValidator : AbstractValidator<RejectAccessRequestCommand>
{
    public RejectAccessRequestCommandValidator(IInputSanitizationService sanitizationService)
    {
        RuleFor(x => x.AccessRequestId)
            .NotEmpty()
            .WithMessage("ERR.AccessRequest.MandatoryID");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .WithMessage("ERR.AccessRequest.MandatoryRejectionMessage")
            .MinimumLength(10)
            .WithMessage("ERR.AccessRequest.LengthMinRejectionMessage")
            .MaximumLength(1000)
            .WithMessage("ERR.AccessRequest.LengthMaxRejectionMessage")
            .SafeDescription(sanitizationService);
    }
}
