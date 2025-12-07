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
    private readonly ClientConnectionDbContext _context;
    private readonly SharePointSettings _sharePointSettings;
    private readonly ILogger<AccessRequestDocumentService> _logger;

    public AccessRequestDocumentService(
        ISharePointGraphService sharePointService,
        ICurrentUserService currentUserService,
        ClientConnectionDbContext context,
        IOptions<SharePointSettings> sharePointSettings,
        ILogger<AccessRequestDocumentService> logger)
    {
        _sharePointService = sharePointService;
        _currentUserService = currentUserService;
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
            var folderPath = $"AccessRequests/{accessRequest.Code}";
            var fileName = SanitizeFileName(document.FileName);

            using var stream = document.OpenReadStream();
            var metadata = new Dictionary<string, object>
            {
                { "RequesterEmail", accessRequest.Email },
                { "AccessRequestCode", accessRequest.Code },
                { "SubmissionDate", DateTime.UtcNow.ToString("yyyy-MM-dd") }
            };

            var webUrl = await _sharePointService.UploadFileAsync(
                _sharePointSettings.SiteId,
                _sharePointSettings.DriveId,
                _sharePointSettings.ListId,
                folderPath,
                stream,
                fileName,
                metadata);

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

            var (fileStream, contentType, downloadFileName) =
                await _sharePointService.DownloadByWebUrlAsync(document.DocumentUrl);

            return new FileDownloaded
            {
                FileStream = fileStream,
                ContentType = contentType,
                FileName = downloadFileName
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

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Concat(fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        return sanitized;
    }
}
