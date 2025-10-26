using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Helpers;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.DisbursementCmd;

public sealed class CreateDisbursementCommandHandler(
    IDisbursementRepository disbursementRepository,
    IDisbursementTypeRepository disbursementTypeRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    ICurrencyRepository currencyRepository,
    IFileValidationService fileValidationService,
    IDisbursementDocumentService disbursementDocumentService,
    IMapper mapper) : IRequestHandler<CreateDisbursementCommand, CreateDisbursementResponse>
{
    private readonly IDisbursementRepository _disbursementRepository = disbursementRepository;
    private readonly IDisbursementTypeRepository _disbursementTypeRepository = disbursementTypeRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ICurrencyRepository _currencyRepository = currencyRepository;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IDisbursementDocumentService _disbursementDocumentService = disbursementDocumentService;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateDisbursementResponse> Handle(CreateDisbursementCommand request, CancellationToken cancellationToken)
    {
        if (request.Documents != null && request.Documents.Count > 0)
        {
            await _fileValidationService.ValidateAndThrowAsync(request.Documents, "Documents");
        }

        var disbursementType = await _disbursementTypeRepository.GetByIdAsync(request.DisbursementTypeId);
        if (disbursementType == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("DisbursementTypeId", "ERR.Disbursement.DisbursementTypeNotExist")
            });

        var currency =  await _currencyRepository.GetByIdAsync(request.CurrencyId);
        if (currency == null)
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("CurrencyId", "ERR.Disbursement.CurrencyNotExist")
            });

        if (!DisbursementTypeCode.IsValid(disbursementType.Code))
            throw new ArgumentException("ERR.Disbursement.DisbursementTypeCodeNotExist");

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
            CurrencyId = request.CurrencyId,
            CreatedByUserId = user.Id,
            CreatedByUser = user,
            CreatedBy = _currentUserService.Email,
            DisbursementA1 = request.DisbursementA1 != null ? MapFormA1Data(request) : null,
            DisbursementA2 = request.DisbursementA2 != null ? MapFormA2Data(request) : null,
            DisbursementA3 = request.DisbursementA3 != null ? MapFormA3Data(request) : null,
            DisbursementB1 = request.DisbursementB1 != null ? MapFormB1Data(request) : null,
        };

        var disbursement = new Disbursement(disbursementNewParam);

        var createdDisbursement = await _disbursementRepository.AddAsync(disbursement, cancellationToken);

        if (request.Documents != null && request.Documents.Count > 0)
        {
            await _disbursementDocumentService.UploadAndAttachDocumentsAsync(
                createdDisbursement,
                request.Documents,
                cancellationToken);
        }

        return new CreateDisbursementResponse
        {
            Disbursement = _mapper.Map<DTOs.DisbursementDto>(createdDisbursement),
            Message = "MSG.DISBURSEMENT.CREATED_SUCCESS"
        };
    }


    private static DisbursementA1 MapFormA1Data(CreateDisbursementCommand request)
    {
        if (request.DisbursementA1 == null)
            throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementA1", "ERR.Disbursement.A1DataRequired")
                    });

        DisbursementA1 formA1 = new DisbursementA1(new DisbursementA1NewParam
        {
            PaymentPurpose = request.DisbursementA1.PaymentPurpose,
            BeneficiaryBpNumber = request.DisbursementA1.BeneficiaryBpNumber,
            BeneficiaryName = request.DisbursementA1.BeneficiaryName,
            BeneficiaryContactPerson = request.DisbursementA1.BeneficiaryContactPerson,
            BeneficiaryAddress = request.DisbursementA1.BeneficiaryAddress,
            BeneficiaryCountryId = request.DisbursementA1.BeneficiaryCountryId,
            BeneficiaryEmail = request.DisbursementA1.BeneficiaryEmail,
            CorrespondentBankName = request.DisbursementA1.CorrespondentBankName,
            CorrespondentBankAddress = request.DisbursementA1.CorrespondentBankAddress,
            CorrespondentBankCountryId = request.DisbursementA1.CorrespondentBankCountryId,
            CorrespondantAccountNumber = request.DisbursementA1.CorrespondantAccountNumber,
            CorrespondentBankSwiftCode = request.DisbursementA1.CorrespondentBankSwiftCode,
            Amount = request.DisbursementA1.Amount,
            SignatoryName = request.DisbursementA1.SignatoryName,
            SignatoryContactPerson = request.DisbursementA1.SignatoryContactPerson,
            SignatoryAddress = request.DisbursementA1.SignatoryAddress,
            SignatoryCountryId = request.DisbursementA1.SignatoryCountryId,
            SignatoryEmail = request.DisbursementA1.SignatoryEmail,
            SignatoryPhone = request.DisbursementA1.SignatoryPhone,
            SignatoryTitle = request.DisbursementA1.SignatoryTitle
        });

        return formA1;
    }

    private static DisbursementA2 MapFormA2Data(CreateDisbursementCommand request)
    {
        if (request.DisbursementA2 == null)
            throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementA2", "ERR.Disbursement.A1DataRequired")
                    });



        DisbursementA2 formA2 = new DisbursementA2(new DisbursementA2NewParam
        {
            ReimbursementPurpose = request.DisbursementA2.ReimbursementPurpose,
            Contractor = request.DisbursementA2.Contractor,
            GoodDescription = request.DisbursementA2.GoodDescription,
            GoodOrginCountryId = request.DisbursementA2.GoodOrginCountryId,
            ContractBorrowerReference = request.DisbursementA2.ContractBorrowerReference,
            ContractAfDBReference = request.DisbursementA2.ContractAfDBReference,
            ContractValue = request.DisbursementA2.ContractValue,
            ContractBankShare = request.DisbursementA2.ContractBankShare,
            ContractAmountPreviouslyPaid = request.DisbursementA2.ContractAmountPreviouslyPaid,
            InvoiceRef = request.DisbursementA2.InvoiceRef,
            InvoiceDate = request.DisbursementA2.InvoiceDate,
            InvoiceAmount = request.DisbursementA2.InvoiceAmount,
            PaymentDateOfPayment = request.DisbursementA2.PaymentDateOfPayment,
            PaymentAmountWithdrawn = request.DisbursementA2.PaymentAmountWithdrawn,
            PaymentEvidenceOfPayment = request.DisbursementA2.PaymentEvidenceOfPayment
        });
        return formA2;
    }

    private static DisbursementA3 MapFormA3Data(CreateDisbursementCommand request)
    {

        if (request.DisbursementA3 == null)
            throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementA3", "ERR.Disbursement.A3DataRequired")
                    });

        DisbursementA3 formA3 = new DisbursementA3(new DisbursementA3NewParam
        {
            PeriodForUtilization = request.DisbursementA3.PeriodForUtilization,
            ItemNumber = request.DisbursementA3.ItemNumber,
            GoodDescription = request.DisbursementA3.GoodDescription,
            GoodOrginCountryId = request.DisbursementA3.GoodOrginCountryId,
            GoodQuantity = request.DisbursementA3.GoodQuantity,
            AnnualBudget = request.DisbursementA3.AnnualBudget,
            BankShare = request.DisbursementA3.BankShare,
            AdvanceRequested = request.DisbursementA3.AdvanceRequested,
            DateOfApproval = request.DisbursementA3.DateOfApproval
        });

        return formA3;

    }

    private static DisbursementB1 MapFormB1Data(CreateDisbursementCommand request)
    {
        if (request.DisbursementB1 == null)
            throw new ValidationException(new[] {
                        new FluentValidation.Results.ValidationFailure("DisbursementB1", "ERR.Disbursement.B1DataRequired")
                    });

        DisbursementB1 fromB1 = new DisbursementB1(new DisbursementB1NewParam
        {
            GuaranteeDetails = request.DisbursementB1.GuaranteeDetails,
            ConfirmingBank = request.DisbursementB1.ConfirmingBank,
            IssuingBankName = request.DisbursementB1.IssuingBankName,
            IssuingBankAdress = request.DisbursementB1.IssuingBankAdress,
            GuaranteeAmount = request.DisbursementB1.GuaranteeAmount,
            ExpiryDate = request.DisbursementB1.ExpiryDate,
            BeneficiaryName = request.DisbursementB1.BeneficiaryName,
            BeneficiaryBPNumber = request.DisbursementB1.BeneficiaryBPNumber,
            BeneficiaryAFDBContract = request.DisbursementB1.BeneficiaryAFDBContract,
            BeneficiaryBankAddress = request.DisbursementB1.BeneficiaryBankAddress,
            BeneficiaryCity = request.DisbursementB1.BeneficiaryCity,
            BeneficiaryCountryId = request.DisbursementB1.BeneficiaryCountryId,
            GoodDescription = request.DisbursementB1.GoodDescription,
            BeneficiaryLcContractRef = request.DisbursementB1.BeneficiaryLcContractRef,
            ExecutingAgencyName = request.DisbursementB1.ExecutingAgencyName,
            ExecutingAgencyContactPerson = request.DisbursementB1.ExecutingAgencyContactPerson,
            ExecutingAgencyAddress = request.DisbursementB1.ExecutingAgencyAddress,
            ExecutingAgencyCity = request.DisbursementB1.ExecutingAgencyCity,
            ExecutingAgencyCountryId = request.DisbursementB1.ExecutingAgencyCountryId,
            ExecutingAgencyEmail = request.DisbursementB1.ExecutingAgencyEmail,
            ExecutingAgencyPhone = request.DisbursementB1.ExecutingAgencyPhone
        });

        return fromB1;
    }
}
