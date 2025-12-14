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

public class OtherDocumentService : IOtherDocumentService
{
    private readonly ISharePointGraphService _sharePointService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ClientConnectionDbContext _context;
    private readonly SharePointSettings _sharePointSettings;
    private readonly ILogger<OtherDocumentService> _logger;

    public OtherDocumentService(
        ISharePointGraphService sharePointService,
        ICurrentUserService currentUserService,
        ClientConnectionDbContext context,
        IOptions<SharePointSettings> sharePointSettings,
        ILogger<OtherDocumentService> logger)
    {
        _sharePointService = sharePointService;
        _currentUserService = currentUserService;
        _context = context;
        _sharePointSettings = sharePointSettings.Value;
        _logger = logger;
    }

    public async Task UploadAndAttachFilesAsync(
        OtherDocument otherDocument,
        IFormFileCollection files,
        CancellationToken cancellationToken = default)
    {
        if (otherDocument == null)
            throw new ArgumentNullException(nameof(otherDocument));

        if (files == null || files.Count == 0)
            throw new ArgumentException("At least one file is required", nameof(files));

        try
        {
            var folderPath = $"OtherDocuments/{otherDocument.SAPCode}/{otherDocument.LoanNumber}/{otherDocument.Year}";

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue;

                var fileName = SanitizeFileName(file.FileName);

                using var stream = file.OpenReadStream();

                var (webUrl, id) = await _sharePointService.UploadFileAsync(
                    _sharePointSettings.SiteId,
                    _sharePointSettings.AccessRequestDriveId,
                    _sharePointSettings.AccessRequestListId,
                    folderPath,
                    stream,
                    fileName,
                    null);

                var fileParam = new OtherDocumentFileNewParam
                {
                    OtherDocumentId = otherDocument.Id,
                    FileName = fileName,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = _currentUserService.Email,
                    CreatedBy = _currentUserService.Email
                };

                var documentFile = new OtherDocumentFile(fileParam);
                otherDocument.AddFile(documentFile);

                _logger.LogInformation(
                    "File {FileName} uploaded successfully for OtherDocument {Id}",
                    fileName, otherDocument.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error uploading files for OtherDocument {Id}",
                otherDocument.Id);
            throw;
        }
    }

    public async Task<FileDownloaded?> DownloadFileAsync(
        Guid otherDocumentId,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        if (otherDocumentId == Guid.Empty)
            throw new ArgumentException("OtherDocument ID is required", nameof(otherDocumentId));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required", nameof(fileName));

        try
        {
            var otherDocument = await _context.OtherDocuments
                .Include(d => d.Files)
                .FirstOrDefaultAsync(d => d.Id == otherDocumentId, cancellationToken);

            if (otherDocument == null)
            {
                _logger.LogWarning(
                    "OtherDocument {Id} not found",
                    otherDocumentId);
                return null;
            }

            var file = otherDocument.Files.FirstOrDefault(f => f.FileName == fileName);

            if (file == null)
            {
                _logger.LogWarning(
                    "File {FileName} not found for OtherDocument {Id}",
                    fileName, otherDocumentId);
                return null;
            }

            var relativePath = string.Join('/', "OtherDocuments", otherDocument.SAPCode,
                otherDocument.LoanNumber, otherDocument.Year, fileName);

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
                "Error downloading file {FileName} for OtherDocument {Id}",
                fileName, otherDocumentId);
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
