using Afdb.ClientConnection.Domain.Enums;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.UserCmd;

public sealed class CreateUserWithCountriesCommandValidator : AbstractValidator<CreateUserWithCountriesCommand>
{
    public CreateUserWithCountriesCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role must be a valid UserRole")
            .Must(role => role == UserRole.Admin || role == UserRole.DO || role == UserRole.DA)
            .WithMessage("Role must be Admin, DO, or DA");

        When(x => x.Role == UserRole.DO || x.Role == UserRole.DA, () =>
        {
            RuleFor(x => x.CountryIds)
                .NotEmpty()
                .WithMessage("CountryIds are required for DO and DA roles")
                .Must(ids => ids.Count > 0)
                .WithMessage("At least one country must be assigned for DO and DA roles");
        });

        When(x => x.Role == UserRole.Admin, () =>
        {
            RuleFor(x => x.CountryIds)
                .Must(ids => ids == null || ids.Count == 0)
                .WithMessage("Admin role cannot have assigned countries");
        });

        RuleForEach(x => x.CountryIds)
            .NotEqual(Guid.Empty)
            .WithMessage("Country ID cannot be empty");
    }
}
