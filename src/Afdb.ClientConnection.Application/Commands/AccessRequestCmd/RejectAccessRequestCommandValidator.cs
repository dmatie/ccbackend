using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public class RejectAccessRequestCommandValidator : AbstractValidator<RejectAccessRequestCommand>
{
    public RejectAccessRequestCommandValidator()
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
            .WithMessage("ERR.AccessRequest.LengthMaxRejectionMessage");
    }
}
