namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IInputSanitizationService
{
    string SanitizeHtml(string input);

    string SanitizeText(string input);

    bool ContainsDangerousContent(string input);

    bool ContainsSqlInjectionPatterns(string input);

    bool ContainsXssPatterns(string input);

    string RemoveDangerousCharacters(string input, bool allowHtml = false);

    bool IsValidFileName(string fileName);

    bool IsValidUrl(string url);
}
