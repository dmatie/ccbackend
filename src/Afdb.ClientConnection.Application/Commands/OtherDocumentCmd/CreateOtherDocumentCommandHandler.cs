using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class CreateOtherDocumentCommandHandler(
    IOtherDocumentRepository otherDocumentRepository,
    IOtherDocumentTypeRepository otherDocumentTypeRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IFileValidationService fileValidationService,
    IOtherDocumentService otherDocumentService,
    ICountryAdminRepository countryAdminRepository,
    IAccessRequestRepository accessRequestRepository,
    IGraphService graphService,
    IMapper mapper)
    : IRequestHandler<CreateOtherDocumentCommand, CreateOtherDocumentResponse>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository = otherDocumentRepository;
    private readonly IOtherDocumentTypeRepository _otherDocumentTypeRepository = otherDocumentTypeRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IOtherDocumentService _otherDocumentService = otherDocumentService;
    private readonly ICountryAdminRepository _countryAdminRepository = countryAdminRepository;
    private readonly IAccessRequestRepository _accessRequestRepository = accessRequestRepository;
    private readonly IGraphService _graphService = graphService;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateOtherDocumentResponse> Handle(
        CreateOtherDocumentCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Files != null && request.Files.Count > 0)
        {
            await _fileValidationService.ValidateFilesAsync(request.Files);
        }

        var otherDocumentType = await _otherDocumentTypeRepository.GetByIdAsync(request.OtherDocumentTypeId);
        if (otherDocumentType == null)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("OtherDocumentTypeId", "ERR.OtherDocument.TypeNotExist")
            });
        }

        if (!otherDocumentType.IsActive)
        {
            throw new ValidationException(new[] {
                new FluentValidation.Results.ValidationFailure("OtherDocumentTypeId", "ERR.OtherDocument.TypeInactive")
            });
        }

        var user = await _userRepository.GetByEmailAsync(_currentUserService.Email)
            ?? throw new NotFoundException("ERR.General.UserNotFound");

        var accessRequest = await _accessRequestRepository.GetByEmailAsync(user.Email)
            ?? throw new NotFoundException("ERR.General.AccessRequestNotFound");

        string[] fifcAdmins = (await _graphService.GetFifcAdmin(cancellationToken)).ToArray() ?? [];
        string[] assignTo = [];

        if (accessRequest.CountryId.HasValue)
        {
            var countryAdmins = await _countryAdminRepository.GetByCountryIdAsync(accessRequest.CountryId.Value, cancellationToken);

            if (countryAdmins != null && countryAdmins.Any())
            {
                assignTo = countryAdmins.Select(ca => ca.User!.Email).ToArray();
            }
        }

        if (assignTo.Length == 0)
        {
            assignTo = fifcAdmins;
        }

        var fileNames = request.Files?.Select(f => f.FileName).ToArray() ?? [];

        var otherDocumentNewParam = new OtherDocumentNewParam
        {
            OtherDocumentTypeId = request.OtherDocumentTypeId,
            Name = request.Name,
            UserId = user.Id,
            Status = OtherDocumentStatus.Submitted,
            SAPCode = request.SAPCode,
            LoanNumber = request.LoanNumber,
            CreatedBy = _currentUserService.Email,
            FileNames = fileNames,
            AssignTo = assignTo,
            AssignCc = fifcAdmins,
            User = user,
            OtherDocumentType = otherDocumentType,
        };

        var otherDocument = new OtherDocument(otherDocumentNewParam);

        if (request.Files != null && request.Files.Count > 0)
        {
            await _otherDocumentService.UploadAndAttachFilesAsync(
                otherDocument,
                request.Files,
                cancellationToken);
        }

        await _otherDocumentRepository.AddAsync(otherDocument, cancellationToken);
        var createdOtherDocument = await _otherDocumentRepository.GetByIdAsync(otherDocument.Id, cancellationToken)
            ?? throw new InvalidOperationException("ERR.OtherDocument.CreationFailed");

        return new CreateOtherDocumentResponse
        {
            OtherDocument = _mapper.Map<DTOs.OtherDocumentDto>(createdOtherDocument),
            Message = "MSG.OtherDocument.CreatedSuccess"
        };
    }
}
