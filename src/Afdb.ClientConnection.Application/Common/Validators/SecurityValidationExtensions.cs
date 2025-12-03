using Afdb.ClientConnection.Application.Common.Interfaces;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Afdb.ClientConnection.Application.Common.Validators;

public static class SecurityValidationExtensions
{
    public static IRuleBuilderOptions<T, string> NoXssContent<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IInputSanitizationService sanitizationService)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || !sanitizationService.ContainsXssPatterns(value))
            .WithMessage("ERR.Validation.XssDetected");
    }

    public static IRuleBuilderOptions<T, string> NoSqlInjection<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IInputSanitizationService sanitizationService)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || !sanitizationService.ContainsSqlInjectionPatterns(value))
            .WithMessage("ERR.Validation.SqlInjectionDetected");
    }

    public static IRuleBuilderOptions<T, string> NoDangerousContent<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IInputSanitizationService sanitizationService)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || !sanitizationService.ContainsDangerousContent(value))
            .WithMessage("ERR.Validation.DangerousContentDetected");
    }

    public static IRuleBuilderOptions<T, string> SafeFileName<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IInputSanitizationService sanitizationService)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || sanitizationService.IsValidFileName(value))
            .WithMessage("ERR.Validation.InvalidFileName");
    }

    public static IRuleBuilderOptions<T, string> SafeUrl<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IInputSanitizationService sanitizationService)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || sanitizationService.IsValidUrl(value))
            .WithMessage("ERR.Validation.InvalidUrl");
    }

    public static IRuleBuilderOptions<T, string> SafeText<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || !ContainsControlCharacters(value))
            .WithMessage("ERR.Validation.InvalidCharacters")
            .Must(value => string.IsNullOrWhiteSpace(value) || !ContainsExcessiveWhitespace(value))
            .WithMessage("ERR.Validation.ExcessiveWhitespace");
    }

    public static IRuleBuilderOptions<T, string> NoScriptTags<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || !ContainsScriptTags(value))
            .WithMessage("ERR.Validation.ScriptTagsNotAllowed");
    }

    public static IRuleBuilderOptions<T, string> AlphanumericWithSpaces<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        bool allowDashes = false,
        bool allowUnderscores = false)
    {
        var pattern = allowDashes && allowUnderscores
            ? @"^[a-zA-Z0-9\s\-_]+$"
            : allowDashes
                ? @"^[a-zA-Z0-9\s\-]+$"
                : allowUnderscores
                    ? @"^[a-zA-Z0-9\s_]+$"
                    : @"^[a-zA-Z0-9\s]+$";

        return ruleBuilder
            .Matches(pattern)
            .WithMessage("ERR.Validation.OnlyAlphanumericAllowed");
    }

    public static IRuleBuilderOptions<T, string> NoLeadingTrailingWhitespace<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || value == value.Trim())
            .WithMessage("ERR.Validation.NoLeadingTrailingWhitespace");
    }

    public static IRuleBuilderOptions<T, string> SafeDescription<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IInputSanitizationService sanitizationService)
    {
        return ruleBuilder
            .NoDangerousContent(sanitizationService)
            .NoScriptTags()
            .SafeText();
    }

    public static IRuleBuilderOptions<T, string> SafeName<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        IInputSanitizationService sanitizationService)
    {
        return ruleBuilder
            .NoXssContent(sanitizationService)
            .NoSqlInjection(sanitizationService)
            .NoLeadingTrailingWhitespace()
            .Must(value => string.IsNullOrWhiteSpace(value) || !ContainsScriptTags(value))
            .WithMessage("ERR.Validation.ScriptTagsNotAllowed");
    }

    private static bool ContainsControlCharacters(string value)
    {
        return value.Any(c => char.IsControl(c) && c != '\n' && c != '\r' && c != '\t');
    }

    private static bool ContainsExcessiveWhitespace(string value)
    {
        return Regex.IsMatch(value, @"\s{10,}");
    }

    private static bool ContainsScriptTags(string value)
    {
        return Regex.IsMatch(value, @"<script[^>]*>.*?</script>", RegexOptions.IgnoreCase);
    }
}
