using Afdb.ClientConnection.Application.Common.Validators;
using FluentValidation;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class AddDisbursementDocumentsCommandValidator : AbstractValidator<AddDisbursementDocumentsCommand>
{
    public AddDisbursementDocumentsCommandValidator()
    {
        RuleFor(x => x.DisbursementId)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.IdRequired");

        RuleFor(x => x.Documents)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.DocumentsRequired")
            .Must(docs => docs.Count <= 3)
            .WithMessage("ERR.Disbursement.MaxThreeDocuments");

        RuleForEach(x => x.Documents)
            .Must(doc => doc != null && doc.Length > 0)
            .WithMessage("ERR.Disbursement.DocumentCannotBeEmpty");
    }
}
