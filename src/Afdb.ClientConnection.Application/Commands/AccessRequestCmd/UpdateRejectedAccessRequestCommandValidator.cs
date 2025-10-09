using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public class UpdateRejectedAccessRequestCommandValidator : AbstractValidator<UpdateRejectedAccessRequestCommand>
{
    public UpdateRejectedAccessRequestCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("ERR.AccessRequest.MandatoryEmail")
            .EmailAddress()
            .WithMessage("ERR.AccessRequest.InvalidEmail")
            .MaximumLength(255)
            .WithMessage("ERR.AccessRequest.LengthMaxEmail");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("ERR.AccessRequest.MandatoryFirstName")
            .MaximumLength(100)
            .WithMessage("ERR.AccessRequest.LengthMaxFirstName");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("ERR.AccessRequest.MandatoryLastName")
            .MaximumLength(100)
            .WithMessage("ERR.AccessRequest.LengthMaxLastName");

        // Validation pour les IDs optionnels (GUIDs valides)
        RuleFor(x => x.FunctionId)
            .Must(BeValidGuid)
            .WithMessage("ERR.AccessRequest.InvalidFunctionId")
            .When(x => x.FunctionId.HasValue);

        RuleFor(x => x.CountryId)
            .Must(BeValidGuid)
            .WithMessage("ERR.AccessRequest.InvalidCountryId")
            .When(x => x.CountryId.HasValue);

        RuleFor(x => x.BusinessProfileId)
            .Must(BeValidGuid)
            .WithMessage("ERR.AccessRequest.InvalidBusinessProfileId")
            .When(x => x.BusinessProfileId.HasValue);
    }

    private static bool BeValidGuid(Guid? guid)
    {
        return guid == null || guid != Guid.Empty;
    }
}