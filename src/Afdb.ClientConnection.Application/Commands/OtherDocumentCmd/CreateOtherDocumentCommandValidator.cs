using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class CreateOtherDocumentCommandValidator : AbstractValidator<CreateOtherDocumentCommand>
{
    public CreateOtherDocumentCommandValidator(IInputSanitizationService sanitizationService)
    {
        RuleFor(x => x.OtherDocumentTypeId)
            .NotEmpty()
            .WithMessage("ERR.OtherDocument.TypeIdRequired");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("ERR.OtherDocument.NameRequired")
            .MaximumLength(200)
            .WithMessage("ERR.OtherDocument.NameTooLong")
            .SafeText();

        RuleFor(x => x.SAPCode)
            .NotEmpty()
            .WithMessage("ERR.OtherDocument.SAPCodeRequired")
            .MaximumLength(50)
            .WithMessage("ERR.OtherDocument.SAPCodeTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x.LoanNumber)
            .NotEmpty()
            .WithMessage("ERR.OtherDocument.LoanNumberRequired")
            .MaximumLength(50)
            .WithMessage("ERR.OtherDocument.LoanNumberTooLong")
            .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);

        RuleFor(x => x.Files)
            .Must(files => files == null || files.Count <= 10)
            .WithMessage("ERR.OtherDocument.TooManyFiles")
            .When(x => x.Files != null);
    }
}
