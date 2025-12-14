using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class SubmitOtherDocumentCommandValidator : AbstractValidator<SubmitOtherDocumentCommand>
{
    public SubmitOtherDocumentCommandValidator()
    {
        RuleFor(x => x.OtherDocumentId)
            .NotEmpty()
            .WithMessage("ERR.OtherDocument.IdRequired");
    }
}
