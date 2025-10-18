using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed class CreateClaimCommandValidator : AbstractValidator<CreateClaimCommand>
{
    public CreateClaimCommandValidator()
    {
        RuleFor(x => x.ClaimTypeId)
            .NotEmpty()
            .WithMessage("ClaimTypeId is required");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment is required")
            .MaximumLength(2000)
            .WithMessage("Comment cannot exceed 2000 characters");
    }
}
