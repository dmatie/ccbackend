using Afdb.ClientConnection.Application.Common.Exceptions;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Afdb.ClientConnection.Infrastructure.Services;

public sealed class DisbursementDocumentService : IDisbursementDocumentService
{
    private readonly ISharePointGraphService _sharePointService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDisbursementRepository _disbursementRepository;
    private readonly SharePointSettings _sharePointSettings;
    private readonly ILogger<DisbursementDocumentService> _logger;

    public DisbursementDocumentService(
        ISharePointGraphService sharePointService,
        ICurrentUserService currentUserService,
        IDisbursementRepository disbursementRepository,
        IOptions<SharePointSettings> sharePointSettings,
        ILogger<DisbursementDocumentService> logger)
    {
        _sharePointService = sharePointService;
        _currentUserService = currentUserService;
        _disbursementRepository = disbursementRepository;
        _sharePointSettings = sharePointSettings.Value;
        _logger = logger;
    }

    public async Task<FileDownloaded?> 
        DownloadAttachDocumentsAsync(string reference, string fileName , CancellationToken cancellationToken = default)
    {
        // Le chemin relatif commence après "Disbursements"
        var relativePath = string.Join('/', reference, fileName);

        (Stream FileContent, string ContentType, string FileName)? downloadResult = await
            _sharePointService.DownloadBySharePointUrlAsync(
            _sharePointSettings.DisbursementDriveId,
            relativePath);

        if(downloadResult == null)
            return null;

        return new FileDownloaded
        {
            FileName = downloadResult?.FileName ?? string.Empty,
            FileContent = downloadResult?.FileContent ?? Stream.Null,
            ContentType = downloadResult?.ContentType ?? string.Empty
        };
    }

    public async Task UploadAndAttachDocumentsAsync(
        Disbursement disbursement,
        IEnumerable<IFormFile> documents,
        CancellationToken cancellationToken = default)
    {
        if (disbursement == null)
            throw new ArgumentNullException(nameof(disbursement));

        if (documents == null || !documents.Any())
            return;

        if (!_sharePointSettings.UseSharePointStorage)
        {
            _logger.LogInformation(
                "SharePoint storage is disabled. Skipping document upload for Disbursement {DisbursementId}",
                disbursement.Id);
            return;
        }

        foreach (var document in documents)
        {
            if (document.Length == 0)
            {
                _logger.LogWarning(
                    "Skipping empty document: {FileName}",
                    document.FileName);
                continue;
            }

            try
            {
                using var stream = document.OpenReadStream();

                var (documentUrl, idDoc) = await _sharePointService.UploadFileAsync(
                    _sharePointSettings.SiteId,
                    _sharePointSettings.DisbursementDriveId,
                    _sharePointSettings.DisbursementListId,
                    disbursement.RequestNumber,
                    stream,
                    document.FileName,
                    null);

                var disbursementDocument = new DisbursementDocument(new DisbursementDocumentNewParam
                {
                    DisbursementId = disbursement.Id,
                    FileName = document.FileName,
                    DocumentUrl = documentUrl,
                    CreatedBy = _currentUserService.Email
                });

                disbursement.AddDocument(disbursementDocument);

                _logger.LogInformation(
                    "Document uploaded successfully: {FileName} for Disbursement {DisbursementId}",
                    document.FileName,
                    disbursement.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to upload document {FileName} for Disbursement {DisbursementId}",
                    document.FileName,
                    disbursement.Id);

                throw new ServerErrorException($"Failed to upload document '{document.FileName}': {ex.Message}");
            }
        }

        await _disbursementRepository.UpdateAsync(disbursement, cancellationToken);

        _logger.LogInformation(
            "All documents uploaded and attached to Disbursement {DisbursementId}",
            disbursement.Id);
    }
}
