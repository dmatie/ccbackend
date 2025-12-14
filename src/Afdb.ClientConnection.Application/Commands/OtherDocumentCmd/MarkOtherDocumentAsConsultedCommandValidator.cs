using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class MarkOtherDocumentAsConsultedCommandValidator : AbstractValidator<MarkOtherDocumentAsConsultedCommand>
{
    public MarkOtherDocumentAsConsultedCommandValidator()
    {
        RuleFor(x => x.OtherDocumentId)
            .NotEmpty()
            .WithMessage("ERR.OtherDocument.IdRequired");
    }
}
