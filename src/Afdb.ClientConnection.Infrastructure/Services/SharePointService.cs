using Afdb.ClientConnection.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using System.Text;

namespace Afdb.ClientConnection.Infrastructure.Services;

public class SharePointGraphService : ISharePointGraphService
{
    private readonly GraphServiceClient _graphClient;

    public SharePointGraphService(GraphServiceClient graphClient)
    {
        _graphClient = graphClient;
    }

    public async Task<string> UploadFileAsync(string _siteId, string _driveId, string folderPath,
        Stream fileStream, string fileName, Dictionary<string, object>? metadata = null)
    {
        try
        {
            // Vérifie/crée le dossier
            var folder = await EnsureFolderExistsAsync(_driveId, folderPath)
                ?? throw new InvalidOperationException("Impossible de créer ou de récupérer le dossier spécifié.");

            // Upload du fichier
            var fileItem = await _graphClient
                .Drives[_driveId]
                .Items[folder.Id]
                .ItemWithPath(fileName)
                .Content
                .PutAsync(fileStream);

            if (fileItem?.ListItem == null)
            {
                throw new InvalidOperationException("Impossible de récupérer l'élément ListItem pour mettre à jour les métadonnées.");
            }


            if (metadata != null && metadata.Count > 0 && fileItem?.ListItem != null)
            {
                // Mise à jour des métadonnées
                await _graphClient
                    .Sites[_siteId]
                    .Lists[_driveId]
                    .Items[fileItem.ListItem.Id]
                    .Fields
                    .PatchAsync(new FieldValueSet
                    {
                        AdditionalData = metadata

                    });
            }

            if(fileItem == null)
            {
                throw new InvalidOperationException("L'upload du fichier a échoué, l'élément DriveItem est null.");
            }

            var webUrl = fileItem.WebUrl 
                ?? throw new InvalidOperationException("WebUrl introuvable dans la réponse Graph.");

            return webUrl;

        }
        catch (ODataError ex)
        {

            string details = $"Graph API error: {ex.Error?.Code} - {ex.Error?.Message}" +
                             $"\nInner: {ex.InnerException?.Message}";
            throw new InvalidOperationException(details, ex);
        }
        catch (ServiceException ex)
        {
            string details = $"Graph Service error: {ex.ResponseStatusCode} - {ex.Message}";
            throw new InvalidOperationException(details, ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erreur lors de l'upload du fichier '{fileName}' dans '{folderPath}'. Détails : " +
                $"{ex.Message}", ex);
        }
    }

    public async Task<(Stream FileStream, string ContentType, string FileName)>
    DownloadByWebUrlAsync(string webUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(webUrl))
                throw new ArgumentException("L'URL du fichier est obligatoire.", nameof(webUrl));

            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(webUrl))
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

            var driveItem = await _graphClient
                .Shares[encoded]
                .DriveItem
                .GetAsync()
                ?? throw new FileNotFoundException("Impossible de résoudre l'élément à partir de l'URL fournie.");

            if(driveItem.ParentReference == null)
                throw new InvalidOperationException("DriveId introuvable pour l'élément spécifié.");

            var stream = await _graphClient
                .Drives[driveItem.ParentReference.DriveId]
                .Items[driveItem.Id]
                .Content
                .GetAsync();

            if (stream == null)
                throw new FileNotFoundException("Le contenu du fichier est introuvable.");

            string contentType = driveItem.File?.MimeType ?? "application/octet-stream";
            string fileName = driveItem.Name ?? "fichier";

            return (stream, contentType, fileName);
        }
        catch (ODataError ex)
        {
            throw new InvalidOperationException(
                $"Graph API error: {ex.Error?.Code} - {ex.Error?.Message}", ex);
        }
    }

    private async Task<DriveItem?> EnsureFolderExistsAsync(string _driveId, string folderPath)
    {
        string[] parts = folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        string currentPath = "";
        DriveItem? parent = await _graphClient.Drives[_driveId].Root.GetAsync();

        foreach (var part in parts)
        {
            currentPath = string.IsNullOrEmpty(currentPath) ? part : $"{currentPath}/{part}";
            DriveItem? existing = null;

            try
            {
                existing = await _graphClient
                    .Drives[_driveId]
                    .Root
                    .ItemWithPath(currentPath)
                    .GetAsync();
            }
            catch (ServiceException ex) when (ex.ResponseStatusCode == 404)
            {
                // pas trouvé → on le crée
            }

            existing ??= await _graphClient
                    .Drives[_driveId]
                    .Root
                    .ItemWithPath(currentPath)
                    .Children
                    .PostAsync(new DriveItem
                    {
                        Name = part,
                        Folder = new Folder()
                    });

            parent = existing!;
        }

        return parent;
    }
}