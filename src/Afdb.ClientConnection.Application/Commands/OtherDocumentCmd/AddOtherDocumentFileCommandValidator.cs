using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class AddOtherDocumentFileCommandValidator : AbstractValidator<AddOtherDocumentFileCommand>
{
    public AddOtherDocumentFileCommandValidator()
    {
        RuleFor(x => x.OtherDocumentId)
            .NotEmpty()
            .WithMessage("ERR.OtherDocument.IdRequired");

        RuleFor(x => x.Files)
            .NotNull()
            .WithMessage("ERR.OtherDocument.FilesRequired")
            .Must(files => files != null && files.Count > 0)
            .WithMessage("ERR.OtherDocument.AtLeastOneFileRequired")
            .Must(files => files == null || files.Count <= 10)
            .WithMessage("ERR.OtherDocument.TooManyFiles");

        RuleFor(x => x.Files)
            .Must(files => files == null || files.All(f => f != null && f.Length > 0))
            .WithMessage("ERR.OtherDocument.EmptyFilesNotAllowed")
            .When(x => x.Files != null && x.Files.Count > 0);

        RuleFor(x => x.Files)
            .Must(files => files == null || files.All(f => f.Length <= 10 * 1024 * 1024))
            .WithMessage("ERR.OtherDocument.FileTooLarge")
            .When(x => x.Files != null && x.Files.Count > 0);
    }
}
