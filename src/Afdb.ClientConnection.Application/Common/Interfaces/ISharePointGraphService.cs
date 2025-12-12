
namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ISharePointGraphService
{
    Task<(Stream FileStream, string ContentType, string FileName)> DownloadByWebUrlAsync(string webUrl);
    Task<(Stream FileStream, string ContentType, string FileName)?> DownloadBySharePointUrlAsync(string driveId, string relativePath);
    Task<(string, string)> UploadFileAsync(string _siteId, string _driveId, string _listId , string folderPath, 
        Stream fileStream, string fileName, Dictionary<string, object>? metadata = null);
}
