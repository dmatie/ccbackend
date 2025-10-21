
namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface ISharePointGraphService
{
    Task<(Stream FileStream, string ContentType, string FileName)> DownloadByWebUrlAsync(string webUrl);
    Task<string> UploadFileAsync(string _siteId, string _driveId, string folderPath, Stream fileStream, string fileName, Dictionary<string, object>? metadata = null);
}
