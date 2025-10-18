using Afdb.ClientConnection.Domain.Enums;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.ClaimCmd;

public sealed class AddClaimResponseCommandValidator : AbstractValidator<AddClaimResponseCommand>
{
    public AddClaimResponseCommandValidator()
    {
        RuleFor(x => x.ClaimId)
            .NotEmpty()
            .WithMessage("ERR.Claim.MandatoryClaimId");

        RuleFor(x => x.Status)
            .Must(s => Enum.IsDefined(typeof(ClaimStatus), s))
            .WithMessage("ERR.Claim.MandatoryStatus");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("ERR.Claim.MandatoryComment")
            .MaximumLength(2000)
            .WithMessage("ERR.Claim.MaxLengthLimit");
    }
}
