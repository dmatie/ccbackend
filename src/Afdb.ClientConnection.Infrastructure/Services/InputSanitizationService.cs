using Afdb.ClientConnection.Application.Common.Interfaces;
using Ganss.Xss;
using System.Text.RegularExpressions;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class InputSanitizationService : IInputSanitizationService
{
    private readonly HtmlSanitizer _htmlSanitizer;

    private static readonly string[] DangerousPatterns =
    [
        @"<script[^>]*>.*?</script>",
        @"javascript:",
        @"on\w+\s*=",
        @"eval\s*\(",
        @"expression\s*\(",
        @"vbscript:",
        @"data:text/html"
    ];

    private static readonly string[] SqlInjectionPatterns =
    [
        @"(\bOR\b|\bAND\b).*?=.*?=",
        @";\s*(DROP|DELETE|INSERT|UPDATE|CREATE|ALTER|EXEC|EXECUTE)\s+",
        @"--.*$",
        @"/\*.*?\*/",
        @"xp_\w+",
        @"sp_\w+",
        @"UNION\s+SELECT",
        @"1\s*=\s*1",
        @"'\s*OR\s*'1'\s*=\s*'1"
    ];

    private static readonly string[] XssPatterns =
    [
        @"<iframe[^>]*>",
        @"<embed[^>]*>",
        @"<object[^>]*>",
        @"<img[^>]*onerror",
        @"<svg[^>]*onload",
        @"<body[^>]*onload"
    ];

    public InputSanitizationService()
    {
        _htmlSanitizer = new HtmlSanitizer();

        _htmlSanitizer.AllowedTags.Clear();
        _htmlSanitizer.AllowedTags.Add("p");
        _htmlSanitizer.AllowedTags.Add("br");
        _htmlSanitizer.AllowedTags.Add("strong");
        _htmlSanitizer.AllowedTags.Add("em");
        _htmlSanitizer.AllowedTags.Add("u");
        _htmlSanitizer.AllowedTags.Add("a");
        _htmlSanitizer.AllowedTags.Add("ul");
        _htmlSanitizer.AllowedTags.Add("ol");
        _htmlSanitizer.AllowedTags.Add("li");

        _htmlSanitizer.AllowedAttributes.Clear();
        _htmlSanitizer.AllowedAttributes.Add("href");
        _htmlSanitizer.AllowedAttributes.Add("title");

        _htmlSanitizer.AllowedSchemes.Clear();
        _htmlSanitizer.AllowedSchemes.Add("http");
        _htmlSanitizer.AllowedSchemes.Add("https");
        _htmlSanitizer.AllowedSchemes.Add("mailto");
    }

    public string SanitizeHtml(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return _htmlSanitizer.Sanitize(input);
    }

    public string SanitizeText(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        input = Regex.Replace(input, @"<[^>]*>", string.Empty);

        input = System.Net.WebUtility.HtmlDecode(input);

        return input.Trim();
    }

    public bool ContainsDangerousContent(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        return ContainsXssPatterns(input) || ContainsSqlInjectionPatterns(input);
    }

    public bool ContainsSqlInjectionPatterns(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        foreach (var pattern in SqlInjectionPatterns)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }

    public bool ContainsXssPatterns(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        foreach (var pattern in XssPatterns)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        foreach (var pattern in DangerousPatterns)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }

    public string RemoveDangerousCharacters(string input, bool allowHtml = false)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        if (allowHtml)
        {
            return SanitizeHtml(input);
        }

        return SanitizeText(input);
    }

    public bool IsValidFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        var invalidChars = Path.GetInvalidFileNameChars();
        if (fileName.Any(c => invalidChars.Contains(c)))
            return false;

        if (fileName.Contains(".."))
            return false;

        if (Regex.IsMatch(fileName, @"^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(\.|$)", RegexOptions.IgnoreCase))
            return false;

        if (ContainsDangerousContent(fileName))
            return false;

        return true;
    }

    public bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
            return false;

        if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            return false;

        if (ContainsXssPatterns(url))
            return false;

        return true;
    }
}
