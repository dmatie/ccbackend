using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Afdb.ClientConnection.Api.Controllers;

/// <summary>
/// Controller de test pour les fonctionnalités SharePoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Protéger les endpoints de test
public class SharePointTestController : ControllerBase
{
    private readonly ISharePointGraphService _sharePointService;
    private readonly SharePointSettings _settings;
    private readonly ILogger<SharePointTestController> _logger;

    public SharePointTestController(
        ISharePointGraphService sharePointService,
        IOptions<SharePointSettings> settings,
        ILogger<SharePointTestController> logger)
    {
        _sharePointService = sharePointService;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Teste l'upload d'un fichier vers SharePoint
    /// </summary>
    /// <param name="file">Fichier à uploader</param>
    /// <param name="folderPath">Chemin du dossier (ex: "test/documents")</param>
    /// <param name="cancellationToken"></param>
    /// <returns>WebUrl du fichier uploadé</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(UploadTestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UploadTestResponse>> TestUpload(
        [FromForm] IFormFile file,
        [FromForm] string folderPath = "test",
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Message = "Aucun fichier fourni ou fichier vide" });
            }

            if (!_settings.UseSharePointStorage)
            {
                return BadRequest(new { Message = "SharePoint storage est désactivé dans la configuration" });
            }

            if (string.IsNullOrEmpty(_settings.SiteId) || string.IsNullOrEmpty(_settings.DriveId))
            {
                return BadRequest(new
                {
                    Message = "Configuration SharePoint incomplète",
                    SiteIdConfigured = !string.IsNullOrEmpty(_settings.SiteId),
                    DriveIdConfigured = !string.IsNullOrEmpty(_settings.DriveId)
                });
            }

            _logger.LogInformation(
                "Test upload: fichier={FileName}, taille={FileSize} bytes, dossier={FolderPath}",
                file.FileName, file.Length, folderPath);

            // Métadonnées de test
            var metadata = new Dictionary<string, object>
            {
                { "Title", $"Test Upload - {file.FileName}" },
                { "Description", $"Fichier de test uploadé le {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC" }
            };

            using var stream = file.OpenReadStream();

            var webUrl = await _sharePointService.UploadFileAsync(
                _settings.SiteId,
                _settings.DriveId,
                folderPath,
                stream,
                file.FileName,
                metadata);

            _logger.LogInformation("Upload réussi: {WebUrl}", webUrl);

            return Ok(new UploadTestResponse
            {
                Success = true,
                Message = "Fichier uploadé avec succès",
                FileName = file.FileName,
                FileSize = file.Length,
                FolderPath = folderPath,
                WebUrl = webUrl,
                UploadedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du test d'upload");

            return StatusCode(500, new
            {
                Success = false,
                Message = "Erreur lors de l'upload",
                Error = ex.Message,
                Details = ex.InnerException?.Message
            });
        }
    }

    /// <summary>
    /// Teste le téléchargement d'un fichier depuis SharePoint via son WebUrl
    /// </summary>
    /// <param name="webUrl">URL web complète du fichier SharePoint</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Le fichier téléchargé</returns>
    [HttpGet("download")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> TestDownload(
        [FromQuery] string webUrl,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(webUrl))
            {
                return BadRequest(new { Message = "Le paramètre webUrl est requis" });
            }

            _logger.LogInformation("Test download: webUrl={WebUrl}", webUrl);

            var (fileStream, contentType, fileName) = await _sharePointService.DownloadByWebUrlAsync(webUrl);

            _logger.LogInformation(
                "Download réussi: fileName={FileName}, contentType={ContentType}",
                fileName, contentType);

            // Copier le stream dans un MemoryStream pour pouvoir le retourner
            var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            // Disposer le stream original
            await fileStream.DisposeAsync();

            return File(memoryStream, contentType, fileName);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "Fichier non trouvé: {WebUrl}", webUrl);

            return NotFound(new
            {
                Success = false,
                Message = "Fichier non trouvé",
                WebUrl = webUrl,
                Error = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du test de download");

            return StatusCode(500, new
            {
                Success = false,
                Message = "Erreur lors du téléchargement",
                Error = ex.Message,
                Details = ex.InnerException?.Message
            });
        }
    }

    /// <summary>
    /// Teste le cycle complet: upload puis download immédiat
    /// </summary>
    /// <param name="file">Fichier à tester</param>
    /// <param name="folderPath">Chemin du dossier de test</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Résultat du test complet</returns>
    [HttpPost("full-cycle")]
    [ProducesResponseType(typeof(FullCycleTestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FullCycleTestResponse>> TestFullCycle(
        [FromForm] IFormFile file,
        [FromForm] string folderPath = "test/cycle",
        CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Message = "Aucun fichier fourni ou fichier vide" });
            }

            _logger.LogInformation("Début test cycle complet: {FileName}", file.FileName);

            // ÉTAPE 1: Upload
            _logger.LogInformation("Étape 1/2: Upload du fichier...");
            var uploadStart = DateTime.UtcNow;

            string webUrl;
            using (var uploadStream = file.OpenReadStream())
            {
                webUrl = await _sharePointService.UploadFileAsync(
                    _settings.SiteId,
                    _settings.DriveId,
                    folderPath,
                    uploadStream,
                    file.FileName,
                    null);
            }

            var uploadDuration = DateTime.UtcNow - uploadStart;
            _logger.LogInformation("Upload terminé en {Duration}ms", uploadDuration.TotalMilliseconds);

            // ÉTAPE 2: Download
            _logger.LogInformation("Étape 2/2: Téléchargement du fichier...");
            var downloadStart = DateTime.UtcNow;

            var (downloadStream, contentType, fileName) = await _sharePointService.DownloadByWebUrlAsync(webUrl);

            // Lire le contenu pour vérifier l'intégrité
            var downloadedContent = new MemoryStream();
            await downloadStream.CopyToAsync(downloadedContent, cancellationToken);
            await downloadStream.DisposeAsync();

            var downloadDuration = DateTime.UtcNow - downloadStart;
            _logger.LogInformation("Download terminé en {Duration}ms", downloadDuration.TotalMilliseconds);

            // Vérification de l'intégrité
            var originalSize = file.Length;
            var downloadedSize = downloadedContent.Length;
            var integrityCheck = originalSize == downloadedSize;

            var totalDuration = DateTime.UtcNow - startTime;

            var response = new FullCycleTestResponse
            {
                Success = true,
                Message = integrityCheck
                    ? "Test cycle complet réussi - Intégrité du fichier vérifiée"
                    : "Test cycle complet terminé - ATTENTION: Tailles différentes!",
                FileName = file.FileName,
                OriginalSize = originalSize,
                DownloadedSize = downloadedSize,
                IntegrityCheck = integrityCheck,
                FolderPath = folderPath,
                WebUrl = webUrl,
                ContentType = contentType,
                UploadDurationMs = (int)uploadDuration.TotalMilliseconds,
                DownloadDurationMs = (int)downloadDuration.TotalMilliseconds,
                TotalDurationMs = (int)totalDuration.TotalMilliseconds,
                TestedAt = DateTime.UtcNow
            };

            _logger.LogInformation(
                "Test cycle complet terminé: Upload={UploadMs}ms, Download={DownloadMs}ms, Total={TotalMs}ms, Intégrité={Integrity}",
                response.UploadDurationMs,
                response.DownloadDurationMs,
                response.TotalDurationMs,
                response.IntegrityCheck);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du test de cycle complet");

            return StatusCode(500, new
            {
                Success = false,
                Message = "Erreur lors du test de cycle complet",
                Error = ex.Message,
                Details = ex.InnerException?.Message,
                DurationMs = (int)(DateTime.UtcNow - startTime).TotalMilliseconds
            });
        }
    }

    /// <summary>
    /// Vérifie la configuration SharePoint
    /// </summary>
    /// <returns>État de la configuration</returns>
    [HttpGet("config")]
    [ProducesResponseType(typeof(ConfigTestResponse), StatusCodes.Status200OK)]
    public ActionResult<ConfigTestResponse> GetConfig()
    {
        var response = new ConfigTestResponse
        {
            UseSharePointStorage = _settings.UseSharePointStorage,
            SiteIdConfigured = !string.IsNullOrEmpty(_settings.SiteId),
            DriveIdConfigured = !string.IsNullOrEmpty(_settings.DriveId),
            SiteId = _settings.SiteId,
            DriveId = _settings.DriveId,
            IsReady = _settings.UseSharePointStorage
                && !string.IsNullOrEmpty(_settings.SiteId)
                && !string.IsNullOrEmpty(_settings.DriveId)
        };

        return Ok(response);
    }
}

// DTOs pour les réponses
public record UploadTestResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public string FolderPath { get; init; } = string.Empty;
    public string WebUrl { get; init; } = string.Empty;
    public DateTime UploadedAt { get; init; }
}

public record FullCycleTestResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public long OriginalSize { get; init; }
    public long DownloadedSize { get; init; }
    public bool IntegrityCheck { get; init; }
    public string FolderPath { get; init; } = string.Empty;
    public string WebUrl { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public int UploadDurationMs { get; init; }
    public int DownloadDurationMs { get; init; }
    public int TotalDurationMs { get; init; }
    public DateTime TestedAt { get; init; }
}

public record ConfigTestResponse
{
    public bool UseSharePointStorage { get; init; }
    public bool SiteIdConfigured { get; init; }
    public bool DriveIdConfigured { get; init; }
    public string SiteId { get; init; } = string.Empty;
    public string DriveId { get; init; } = string.Empty;
    public bool IsReady { get; init; }
}
