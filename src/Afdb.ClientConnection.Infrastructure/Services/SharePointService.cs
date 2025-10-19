using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;

namespace Afdb.ClientConnection.Infrastructure.Services;

public class SharePointGraphService
{
    private readonly GraphServiceClient _graphClient;

    public SharePointGraphService(GraphServiceClient graphClient)
    {
        _graphClient = graphClient;
    }

    public async Task UploadFileAsync(string _siteId, string _driveId, string folderPath,
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