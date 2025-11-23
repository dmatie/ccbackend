using FluentValidation;

namespace Afdb.ClientConnection.Application.Queries.DisbursementQrs;

public class GetFileUploadedQueryValidator : AbstractValidator<GetFileUploadedQuery>
{
    public GetFileUploadedQueryValidator()
    {
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.MandatoryReferenceNumber");
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("ERR.Disbursement.MandatoryFileName");
    }
} 
