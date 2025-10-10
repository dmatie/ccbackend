using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed class AddClaimResponseCommandValidator : AbstractValidator<AddClaimResponseCommand>
{
    public AddClaimResponseCommandValidator()
    {
        RuleFor(x => x.ClaimId)
            .NotEmpty()
            .WithMessage("ClaimId is required");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid Status value");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment is required")
            .MaximumLength(2000)
            .WithMessage("Comment must not exceed 2000 characters");
    }
}
