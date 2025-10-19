using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class CreateDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IDisbursementTypeRepository disbursementTypeRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<CreateDisbursementCommand, CreateDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IDisbursementTypeRepository _disbursementTypeRepository = disbursementTypeRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateDisbursementResponse> Handle(CreateDisbursementCommand request, CancellationToken cancellationToken)
    {
        var disbursementType = await _disbursementTypeRepository.GetByIdAsync(request.DisbursementTypeId);
        if (disbursementType == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("DisbursementTypeId", "ERR.Disbursement.DisbursementTypeNotExist")
            });

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        var requestNumber = await _disbursementRepository.GenerateRequestNumberAsync(cancellationToken);

        var disbursementNewParam = new DisbursementNewParam
        {
            RequestNumber = requestNumber,
            SapCodeProject = request.SapCodeProject,
            LoanGrantNumber = request.LoanGrantNumber,
            DisbursementTypeId = request.DisbursementTypeId,
            DisbursementType = disbursementType,
            CreatedByUserId = user.Id,
            CreatedByUser = user,
            CreatedBy = _currentUserService.Email
        };

        MapFormData(request, disbursementType.Code, disbursementNewParam);

        var disbursement = new Disbursement(disbursementNewParam);

        var createdDisbursement = await _disbursementRepository.AddAsync(disbursement, cancellationToken);

        return new CreateDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(createdDisbursement),
            Message = "MSG.DISBURSEMENT.CREATED_SUCCESS"
        };
    }

    private void MapFormData(CreateDisbursementCommand request, string typeCode, DisbursementNewParam param)
    {
        switch (typeCode.ToUpper())
        {
            case "A1":
                if (request.DisbursementA1 == null)
                    throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementA1", "ERR.Disbursement.A1DataRequired")
                    });

                param.DisbursementA1 = new DisbursementA1(new DisbursementA1NewParam
                {
                    PaymentPurpose = request.DisbursementA1.PaymentPurpose,
                    BeneficiaryName = request.DisbursementA1.BeneficiaryName,
                    BeneficiaryAddress = request.DisbursementA1.BeneficiaryAddress,
                    BeneficiaryBankName = request.DisbursementA1.BeneficiaryBankName,
                    BeneficiaryBankAddress = request.DisbursementA1.BeneficiaryBankAddress,
                    BeneficiaryAccountNumber = request.DisbursementA1.BeneficiaryAccountNumber,
                    BeneficiarySwiftCode = request.DisbursementA1.BeneficiarySwiftCode,
                    Amount = new Money(request.DisbursementA1.Amount, request.DisbursementA1.CurrencyCode),
                    IntermediaryBankName = request.DisbursementA1.IntermediaryBankName,
                    IntermediaryBankSwiftCode = request.DisbursementA1.IntermediaryBankSwiftCode,
                    SpecialInstructions = request.DisbursementA1.SpecialInstructions,
                    CreatedBy = _currentUserService.Email
                });
                break;

            case "A2":
                if (request.DisbursementA2 == null)
                    throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementA2", "ERR.Disbursement.A2DataRequired")
                    });

                param.DisbursementA2 = new DisbursementA2(new DisbursementA2NewParam
                {
                    ReimbursementPurpose = request.DisbursementA2.ReimbursementPurpose,
                    ClaimantName = request.DisbursementA2.ClaimantName,
                    ClaimantAddress = request.DisbursementA2.ClaimantAddress,
                    ClaimantBankName = request.DisbursementA2.ClaimantBankName,
                    ClaimantBankAddress = request.DisbursementA2.ClaimantBankAddress,
                    ClaimantAccountNumber = request.DisbursementA2.ClaimantAccountNumber,
                    ClaimantSwiftCode = request.DisbursementA2.ClaimantSwiftCode,
                    Amount = new Money(request.DisbursementA2.Amount, request.DisbursementA2.CurrencyCode),
                    ExpenseDate = request.DisbursementA2.ExpenseDate,
                    ExpenseDescription = request.DisbursementA2.ExpenseDescription,
                    SupportingDocuments = request.DisbursementA2.SupportingDocuments,
                    SpecialInstructions = request.DisbursementA2.SpecialInstructions,
                    CreatedBy = _currentUserService.Email
                });
                break;

            case "A3":
                if (request.DisbursementA3 == null)
                    throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementA3", "ERR.Disbursement.A3DataRequired")
                    });

                param.DisbursementA3 = new DisbursementA3(new DisbursementA3NewParam
                {
                    AdvancePurpose = request.DisbursementA3.AdvancePurpose,
                    RecipientName = request.DisbursementA3.RecipientName,
                    RecipientAddress = request.DisbursementA3.RecipientAddress,
                    RecipientBankName = request.DisbursementA3.RecipientBankName,
                    RecipientBankAddress = request.DisbursementA3.RecipientBankAddress,
                    RecipientAccountNumber = request.DisbursementA3.RecipientAccountNumber,
                    RecipientSwiftCode = request.DisbursementA3.RecipientSwiftCode,
                    Amount = new Money(request.DisbursementA3.Amount, request.DisbursementA3.CurrencyCode),
                    ExpectedUsageDate = request.DisbursementA3.ExpectedUsageDate,
                    JustificationForAdvance = request.DisbursementA3.JustificationForAdvance,
                    RepaymentTerms = request.DisbursementA3.RepaymentTerms,
                    SpecialInstructions = request.DisbursementA3.SpecialInstructions,
                    CreatedBy = _currentUserService.Email
                });
                break;

            case "B1":
                if (request.DisbursementB1 == null)
                    throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementB1", "ERR.Disbursement.B1DataRequired")
                    });

                param.DisbursementB1 = new DisbursementB1(new DisbursementB1NewParam
                {
                    GuaranteePurpose = request.DisbursementB1.GuaranteePurpose,
                    BeneficiaryName = request.DisbursementB1.BeneficiaryName,
                    BeneficiaryAddress = request.DisbursementB1.BeneficiaryAddress,
                    BeneficiaryBankName = request.DisbursementB1.BeneficiaryBankName,
                    BeneficiaryBankAddress = request.DisbursementB1.BeneficiaryBankAddress,
                    GuaranteeAmount = new Money(request.DisbursementB1.GuaranteeAmount, request.DisbursementB1.CurrencyCode),
                    ValidityStartDate = request.DisbursementB1.ValidityStartDate,
                    ValidityEndDate = request.DisbursementB1.ValidityEndDate,
                    GuaranteeTermsAndConditions = request.DisbursementB1.GuaranteeTermsAndConditions,
                    SpecialInstructions = request.DisbursementB1.SpecialInstructions,
                    CreatedBy = _currentUserService.Email
                });
                break;

            default:
                throw new ValidationException(new[] {
                    new FluentValidation.Results.ValidationFailure("DisbursementTypeId", "ERR.Disbursement.InvalidDisbursementType")
                });
        }
    }
}
