using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Domain.Entities;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class SapTokenProviderService : ISapTokenProviderService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly ILogger<SapTokenProviderService> _logger;

    public SapTokenProviderService(IConfiguration configuration, HttpClient httpClient, 
        ILogger<SapTokenProviderService> logger)
    {
        _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;

    }

    public async Task<string> GetAccessTokenAsync()
    {
        try
        {
            string url = _config["Sap:TokenUrl"]!;
            string sapClientIdKvKey = _config["KeyVault:SapClientIdKeyName"]!;
            string sapSecretKvKey = _config["KeyVault:SapSecretKeyName"]!;

            string clientId = _config[sapClientIdKvKey]!;
            string clientSecret = _config[sapSecretKvKey]!;

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"response_type", "token"}
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(content);

            Token token = new Token
            {
                AccessToken = json.RootElement.GetProperty("access_token").GetString()!,
                TokenType = json.RootElement.GetProperty("token_type").GetString()!,
                ExpiresIn = json.RootElement.GetProperty("expires_in").GetInt32(),
                Scope = json.RootElement.TryGetProperty("scope", out var scopeProp) ? scopeProp.GetString() : null
            };

            return token.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when getting SAP Token");
            throw new InvalidOperationException($"Error when getting SAP Token: {ex.Message}", ex);
        }
    }
}
