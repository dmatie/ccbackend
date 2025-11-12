using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class PowerAutomateService : IPowerAutomateService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PowerAutomateService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public PowerAutomateService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<PowerAutomateService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };
    }

    public async Task NotifyClaimCreatedAsync(ClaimCreatedEvent claimCreatedEvent)
    {
        ArgumentNullException.ThrowIfNull(claimCreatedEvent);

        var url = _configuration["PowerAutomate:ClaimCreatedUrl"];

        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("PowerAutomate:ClaimCreatedUrl is not configured. Skipping notification for ClaimId: {ClaimId}", claimCreatedEvent.ClaimId);
            return;
        }

        try
        {
            _logger.LogInformation(
                "Sending ClaimCreated notification to PowerAutomate for ClaimId: {ClaimId}",
                claimCreatedEvent.ClaimId);

            var payload = new
            {
                claimId = claimCreatedEvent.ClaimId,
                claimTypeEn = claimCreatedEvent.ClaimTypeEn,
                claimTypeFr = claimCreatedEvent.ClaimTypeFr,
                comment = claimCreatedEvent.Comment,
                country = claimCreatedEvent.Country,
                author = new
                {
                    firstName = claimCreatedEvent.AuthorFirstName,
                    lastName = claimCreatedEvent.AuthorLastName,
                    email = claimCreatedEvent.AuthorEmail
                },
                assignToEmail = claimCreatedEvent.AssignToEmail,
                assignCcEmail = claimCreatedEvent.AssignCcEmail,
                timestamp = DateTime.UtcNow
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload, _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError(
                    "PowerAutomate ClaimCreated notification failed. StatusCode: {StatusCode}, ClaimId: {ClaimId}, Error: {Error}",
                    response.StatusCode,
                    claimCreatedEvent.ClaimId,
                    errorContent);

                throw new HttpRequestException(
                    $"PowerAutomate notification failed with status {response.StatusCode}: {errorContent}");
            }

            _logger.LogInformation(
                "Successfully sent ClaimCreated notification to PowerAutomate for ClaimId: {ClaimId}",
                claimCreatedEvent.ClaimId);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP error while sending ClaimCreated notification to PowerAutomate for ClaimId: {ClaimId}",
                claimCreatedEvent.ClaimId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while sending ClaimCreated notification to PowerAutomate for ClaimId: {ClaimId}",
                claimCreatedEvent.ClaimId);
            throw new InvalidOperationException("An error occurred while sending notification to PowerAutomate", ex);
        }
    }

    public async Task NotifyClaimResponseAddedAsync(ClaimProcessAddedEvent claimResponseAddedEvent)
    {
        ArgumentNullException.ThrowIfNull(claimResponseAddedEvent);

        var url = _configuration["PowerAutomate:ClaimResponseUrl"];

        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("PowerAutomate:ClaimResponseUrl is not configured. Skipping notification for ClaimId: {ClaimId}", claimResponseAddedEvent.ClaimId);
            return;
        }

        try
        {
            _logger.LogInformation(
                "Sending ClaimResponseAdded notification to PowerAutomate for ClaimId: {ClaimId}",
                claimResponseAddedEvent.ClaimId);

            var payload = new
            {
                claimId = claimResponseAddedEvent.ClaimId,
                claimTypeEn = claimResponseAddedEvent.ClaimTypeEn,
                claimTypeFr = claimResponseAddedEvent.ClaimTypeFr,
                comment = claimResponseAddedEvent.Comment,
                country = claimResponseAddedEvent.Country,
                author = new
                {
                    firstName = claimResponseAddedEvent.AuthorFirstName,
                    lastName = claimResponseAddedEvent.AuthorLastName,
                    email = claimResponseAddedEvent.AuthorEmail
                },
                process = new
                {
                    comment = claimResponseAddedEvent.ProcessComment,
                    author = new
                    {
                        firstName = claimResponseAddedEvent.ProcessAuthorFirstName,
                        lastName = claimResponseAddedEvent.ProcessAuthorLastName
                    },
                    status = claimResponseAddedEvent.ProcessStatus
                },
                timestamp = DateTime.UtcNow
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload, _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError(
                    "PowerAutomate ClaimResponseAdded notification failed. StatusCode: {StatusCode}, ClaimId: {ClaimId}, Error: {Error}",
                    response.StatusCode,
                    claimResponseAddedEvent.ClaimId,
                    errorContent);

                throw new HttpRequestException(
                    $"PowerAutomate notification failed with status {response.StatusCode}: {errorContent}");
            }

            _logger.LogInformation(
                "Successfully sent ClaimResponseAdded notification to PowerAutomate for ClaimId: {ClaimId}",
                claimResponseAddedEvent.ClaimId);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP error while sending ClaimResponseAdded notification to PowerAutomate for ClaimId: {ClaimId}",
                claimResponseAddedEvent.ClaimId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while sending ClaimResponseAdded notification to PowerAutomate for ClaimId: {ClaimId}",
                claimResponseAddedEvent.ClaimId);
            throw new InvalidOperationException("An error occurred while sending notification to PowerAutomate", ex);
        }
    }

    public async Task NotifyOtpCreatedAsync(string customerEmail, string code)
    {
        ArgumentNullException.ThrowIfNull(customerEmail);
        ArgumentNullException.ThrowIfNull(code);

        string otpSigKeyName = _configuration["KeyVault:OtpSendSecretKeyName"]!;
        string sigValue = _configuration[otpSigKeyName]!;
        var url = _configuration["PowerAutomate:OtpSendUrl"];


        if (string.IsNullOrWhiteSpace(sigValue))
        {
            _logger.LogWarning("PowerAutomate:SendOtp Key is not configured. Skipping Otp notification code to Email: {Email}",
                customerEmail);
            return;
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("PowerAutomate:SendOtp URL is not configured. Skipping Otp notification code to Email: {Email}",
                customerEmail);
            return;
        }

        try
        {
            _logger.LogInformation(
                "Sending Otp notification to PowerAutomate for Email: {Email}",
                customerEmail);

            var payload = new
            {
                email = customerEmail,
                code = code
            };

            url = $"{url}{sigValue}";

            var response = await _httpClient.PostAsJsonAsync(url, payload, _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError(
                    "PowerAutomate Otp notification failed. StatusCode: {StatusCode}, Email: {Email}, Error: {Error}",
                    response.StatusCode,
                    customerEmail,
                    errorContent);

                throw new HttpRequestException(
                    $"PowerAutomate notification failed with status {response.StatusCode}: {errorContent}");
            }

            _logger.LogInformation(
                "Successfully sent Otp notification to PowerAutomate for Email: {Email}",
                customerEmail);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP error while sending Otp notification to PowerAutomate for Email: {Email}",
                customerEmail);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while sending Otp notification to PowerAutomate for Email: {Email}",
                customerEmail);
            throw new InvalidOperationException("An error occurred while sending notification to PowerAutomate", ex);
        }
    }

    public async Task TriggerNotificationFlowAsync(object payload, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(payload);

        var flowSigKeyName = _configuration["KeyVault:NotificationFlowSecretKeyName"];
        if (string.IsNullOrWhiteSpace(flowSigKeyName))
        {
            _logger.LogWarning("PowerAutomate:NotificationFlowUrl Sig Key is not configured. Skipping notification.");
            return;
        }

        var sigValue = _configuration[flowSigKeyName];
        if (string.IsNullOrWhiteSpace(sigValue))
        {
            _logger.LogWarning("PowerAutomate:NotificationFlowUrl Sig Key is not found in keyvault. Skipping notification.");
            return;
        }

        var url = _configuration["PowerAutomate:NotificationFlowUrl"];

        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("PowerAutomate:NotificationFlowUrl is not configured. Skipping notification.");
            return;
        }

        try
        {
            _logger.LogInformation("Triggering PowerAutomate notification flow");

            url = $"{url}{sigValue}";

            Console.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }));

            var response = await _httpClient.PostAsJsonAsync(url, payload, _jsonOptions, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(
                    "PowerAutomate notification flow failed. StatusCode: {StatusCode}, Error: {Error}",
                    response.StatusCode,
                    errorContent);
            }
            else
            {
                _logger.LogInformation("Successfully triggered PowerAutomate notification flow");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "HTTP error while triggering PowerAutomate notification flow");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while triggering PowerAutomate notification flow");
        }
    }
}
