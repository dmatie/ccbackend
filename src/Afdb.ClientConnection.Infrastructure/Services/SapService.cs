using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Application.DTOs;
using Afdb.ClientConnection.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Afdb.ClientConnection.Infrastructure.Services;

internal sealed class SapService : ISapService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;


    public SapService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public Task<IEnumerable<ProjectDto>> GetProjectsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SapProjectData>> GetProjectsByCountryAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        var baseUrl = _configuration["Sap:ProjectDataUrl"];
        if (string.IsNullOrEmpty(baseUrl))
            throw new InvalidOperationException("ERR.Sap.ProjectDataUrlMissing");

        var apiKeySecretKeyValue = _configuration["KeyVault:SapProxyAPIKeyValue"];
        if (string.IsNullOrEmpty(apiKeySecretKeyValue))
            throw new InvalidOperationException("ERR.KeyVault.SapProxyAPIKeyValueMissing");

        var apiKeySecretKeyName = _configuration["KeyVault:SapProxyAPIKeyName"];
        if (string.IsNullOrEmpty(apiKeySecretKeyName))
            throw new InvalidOperationException("ERR.KeyVault.SapProxyAPIKeyNameMissing");


        var apiKeyName = _configuration[apiKeySecretKeyName];
        if (string.IsNullOrEmpty(apiKeyName))
            throw new InvalidOperationException("ERR.KeyVault.ApiKeyMissing");

        var apiKeyValue = _configuration[apiKeySecretKeyValue];
        if (string.IsNullOrEmpty(apiKeyValue))
            throw new InvalidOperationException("ERR.KeyVault.ApiKeyValueMissing");

        var url = $"{baseUrl}{countryCode}";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add(apiKeyName, apiKeyValue);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
       
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        // Désérialisation JSON -> liste d’objets typés
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        List<SapProjectData> projects = JsonSerializer.Deserialize<List<SapProjectData>>(content, options) ?? [];

        return projects;
    }
}
