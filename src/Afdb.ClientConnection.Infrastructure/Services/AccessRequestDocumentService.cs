using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Infrastructure.Data;
using Afdb.ClientConnection.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Afdb.ClientConnection.Infrastructure.Services;

public class AccessRequestDocumentService : IAccessRequestDocumentService
{
    private readonly ISharePointGraphService _sharePointService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPdfGenerationService _pdfGenerationService;
    private readonly ClientConnectionDbContext _context;
    private readonly SharePointSettings _sharePointSettings;
    private readonly ILogger<AccessRequestDocumentService> _logger;

    public AccessRequestDocumentService(
        ISharePointGraphService sharePointService,
        ICurrentUserService currentUserService,
        IPdfGenerationService pdfGenerationService,
        ClientConnectionDbContext context,
        IOptions<SharePointSettings> sharePointSettings,
        ILogger<AccessRequestDocumentService> logger)
    {
        _sharePointService = sharePointService;
        _currentUserService = currentUserService;
        _pdfGenerationService = pdfGenerationService;
        _context = context;
        _sharePointSettings = sharePointSettings.Value;
        _logger = logger;
    }

    public async Task UploadAndAttachDocumentAsync(
        AccessRequest accessRequest,
        IFormFile document,
        CancellationToken cancellationToken = default)
    {
        if (accessRequest == null)
            throw new ArgumentNullException(nameof(accessRequest));

        if (document == null || document.Length == 0)
            throw new ArgumentException("Document is required", nameof(document));

        try
        {
            var folderPath = $"{accessRequest.Code}/Signed";
            var fileName = SanitizeFileName(document.FileName);

            using var stream = document.OpenReadStream();

            var (webUrl, id) = await _sharePointService.UploadFileAsync(
                _sharePointSettings.SiteId,
                _sharePointSettings.AccessRequestDriveId,
                _sharePointSettings.AccessRequestListId,
                folderPath,
                stream,
                fileName,
                null);

            var documentParam = new AccessRequestDocumentNewParam
            {
                AccessRequestId = accessRequest.Id,
                FileName = fileName,
                DocumentUrl = webUrl,
                CreatedBy = _currentUserService.Email
            };

            var documentEntity = new AccessRequestDocument(documentParam);
            accessRequest.Documents.Add(documentEntity);

            _logger.LogInformation(
                "Document {FileName} uploaded successfully for AccessRequest {Code}",
                fileName, accessRequest.Code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error uploading document for AccessRequest {Code}",
                accessRequest.Code);
            throw;
        }
    }

    public async Task<FileDownloaded?> DownloadDocumentAsync(
        string code,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("AccessRequest code is required", nameof(code));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required", nameof(fileName));

        try
        {
            var document = await _context.AccessRequestDocuments
                .Include(d => d.AccessRequest)
                .FirstOrDefaultAsync(d =>
                    d.AccessRequest!.Code == code &&
                    d.FileName == fileName,
                    cancellationToken);

            if (document == null)
            {
                _logger.LogWarning(
                    "Document {FileName} not found for AccessRequest {Code}",
                    fileName, code);
                return null;
            }

            var relativePath = string.Join('/', code, "Signed", fileName);

            (Stream FileContent, string ContentType, string FileName)? downloadResult = await
                _sharePointService.DownloadBySharePointUrlAsync(
                _sharePointSettings.AccessRequestDriveId,
                relativePath);

            if (downloadResult == null)
                return null;

            return new FileDownloaded
            {
                FileName = downloadResult?.FileName ?? string.Empty,
                FileContent = downloadResult?.FileContent ?? Stream.Null,
                ContentType = downloadResult?.ContentType ?? string.Empty
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error downloading document {FileName} for AccessRequest {Code}",
                fileName, code);
            throw;
        }
    }

    public async Task<(string frenchUrl, string idFr, string englishUrl, string idEn)> GenerateAndUploadAuthorizationFormsAsync(
       string code,
       string firstName,
       string lastName,
       string email,
       string functionName,
       string functionNameFr,
       List<string> projects,
       CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("AccessRequest code is required", nameof(code));

        try
        {
            var frenchPdf = _pdfGenerationService.GenerateAuthorizationForm(
                firstName, lastName, email, functionNameFr, projects, "fr");

            var englishPdf = _pdfGenerationService.GenerateAuthorizationForm(
                firstName, lastName, email, functionName, projects, "en");

            var folderPath = $"{code}/Forms";
            var frenchFileName = $"Formulaire_Autorisation_FR_{code}.pdf";
            var englishFileName = $"Authorization_Form_EN_{code}.pdf";

            using var frenchStream = new MemoryStream(frenchPdf);
             var(frenchPdfUrl, idFr) = await _sharePointService.UploadFileAsync(
                _sharePointSettings.SiteId,
                _sharePointSettings.AccessRequestDriveId,
                _sharePointSettings.AccessRequestListId,
                folderPath,
                frenchStream,
                frenchFileName,
                null);

            using var englishStream = new MemoryStream(englishPdf);
            var(englishPdfUrl, idEn) = await _sharePointService.UploadFileAsync(
                _sharePointSettings.SiteId,
                _sharePointSettings.AccessRequestDriveId,
                _sharePointSettings.AccessRequestListId,
                folderPath,
                englishStream,
                englishFileName,
                null);

            _logger.LogInformation(
                "Authorization forms (FR/EN) generated and uploaded successfully for AccessRequest {Code}",
                code);

            return (frenchPdfUrl, idFr, englishPdfUrl, idEn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error generating and uploading authorization forms for AccessRequest {Code}",
                code);
            throw;
        }
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Concat(fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        return sanitized;
    }
}
