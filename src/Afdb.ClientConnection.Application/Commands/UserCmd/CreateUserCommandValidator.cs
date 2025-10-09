using Afdb.ClientConnection.Domain.Enums;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("")
            .EmailAddress()
            .WithMessage("ERR.User.MandatoryEmail")
            .MaximumLength(255)
            .WithMessage("ERR.User.LengthMaxEmail");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("ERR.User.MandatoryFirstName")
            .MaximumLength(100)
            .WithMessage("ERR.User.LengthMaxFirstName");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("ERR.User.MandatoryLastName")
            .MaximumLength(100)
            .WithMessage("ERR.User.LengthMaxLastName");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("ERR.User.InvalidRole");

        RuleFor(x => x.EntraIdObjectId)
            .MaximumLength(255)
            .WithMessage("ERR.User.LengthMaxEntraID")
            .When(x => !string.IsNullOrEmpty(x.EntraIdObjectId));

        RuleFor(x => x.OrganizationName)
            .MaximumLength(200)
            .WithMessage("ERR.User.LengthMaxOrganization")
            .When(x => !string.IsNullOrEmpty(x.OrganizationName));

        // Validation conditionnelle : les utilisateurs externes doivent avoir une organisation
        RuleFor(x => x.OrganizationName)
            .NotEmpty()
            .WithMessage("ERR.User.MandatoryOrganization")
            .When(x => x.Role == UserRole.ExternalUser);
    }
}