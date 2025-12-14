using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using AutoMapper;
using MediatR;

namespace Afdb.ClientConnection.Application.Commands.OtherDocumentCmd;

public sealed class CreateOtherDocumentCommandHandler(
    IOtherDocumentRepository otherDocumentRepository,
    IOtherDocumentTypeRepository otherDocumentTypeRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IFileValidationService fileValidationService,
    IOtherDocumentService otherDocumentService,
    IMapper mapper)
    : IRequestHandler<CreateOtherDocumentCommand, CreateOtherDocumentResponse>
{
    private readonly IOtherDocumentRepository _otherDocumentRepository = otherDocumentRepository;
    private readonly IOtherDocumentTypeRepository _otherDocumentTypeRepository = otherDocumentTypeRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IFileValidationService _fileValidationService = fileValidationService;
    private readonly IOtherDocumentService _otherDocumentService = otherDocumentService;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateOtherDocumentResponse> Handle(
        CreateOtherDocumentCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Files != null && request.Files.Count > 0)
        {
            await _fileValidationService.ValidateAndThrowAsync(request.Files, "Files");
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

        var otherDocumentNewParam = new OtherDocumentNewParam
        {
            OtherDocumentTypeId = request.OtherDocumentTypeId,
            Name = request.Name,
            Year = request.Year,
            UserId = user.Id,
            Status = OtherDocumentStatus.Draft,
            SAPCode = request.SAPCode,
            LoanNumber = request.LoanNumber,
            CreatedBy = _currentUserService.Email
        };

        var otherDocument = new OtherDocument(otherDocumentNewParam);

        await _otherDocumentRepository.AddAsync(otherDocument, cancellationToken);

        if (request.Files != null && request.Files.Count > 0)
        {
            await _otherDocumentService.UploadAndAttachFilesAsync(
                otherDocument,
                request.Files,
                cancellationToken);

            await _otherDocumentRepository.UpdateAsync(otherDocument, cancellationToken);
        }

        return new CreateOtherDocumentResponse
        {
            OtherDocument = _mapper.Map<DTOs.OtherDocumentDto>(otherDocument),
            Message = "MSG.OtherDocument.CreatedSuccess"
        };
    }
}
