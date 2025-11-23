using Afdb.ClientConnection.Api.Attributes;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text;
using System.Text.Json;

namespace Afdb.ClientConnection.Api.Middleware;

/// <summary>
/// Middleware pour décrypter automatiquement les payloads des requêtes entrantes
/// </summary>
public class PayloadDecryptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PayloadDecryptionMiddleware> _logger;

    public PayloadDecryptionMiddleware(
        RequestDelegate next,
        ILogger<PayloadDecryptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IPayloadEncryptionService encryptionService)
    {
        // Si l'encryption est complètement désactivée, on skip
        if (!encryptionService.IsEnabled)
        {
            await _next(context);
            return;
        }

        // Vérifie si l'endpoint nécessite la decryption
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        // Vérifie les attributs
        var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        var noEncryption = endpoint.Metadata.GetMetadata<NoEncryptionAttribute>();
        var encryptedPayload = endpoint.Metadata.GetMetadata<EncryptedPayloadAttribute>();

        // Si [NoEncryption] est présent, on skip
        if (noEncryption != null)
        {
            await _next(context);
            return;
        }

        // Détermine si on doit décrypter basé sur la configuration et les attributs
        var shouldDecrypt = false;

        // 1. Vérifie la configuration globale pour ce path
        if (encryptionService.ShouldEncrypt(context.Request.Path))
        {
            shouldDecrypt = true;
        }

        // 2. Vérifie l'attribut [EncryptedPayload]
        if (encryptedPayload != null && encryptedPayload.EncryptRequest)
        {
            shouldDecrypt = true;
        }

        // Si pas de decryption nécessaire, on skip
        if (!shouldDecrypt)
        {
            await _next(context);
            return;
        }

        // Vérifie que c'est une requête avec body (POST, PUT, PATCH)
        if (context.Request.Method != HttpMethod.Post.Method &&
            context.Request.Method != HttpMethod.Put.Method &&
            context.Request.Method != "PATCH")
        {
            await _next(context);
            return;
        }

        // Vérifie qu'il y a un body
        if (!context.Request.HasFormContentType && context.Request.ContentLength == 0)
        {
            await _next(context);
            return;
        }

        try
        {
            // Active le buffering pour pouvoir lire le body plusieurs fois
            context.Request.EnableBuffering();

            // Lit le body encrypté
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);

            var encryptedBody = await reader.ReadToEndAsync();

            // Reset position pour permettre la lecture par le framework
            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(encryptedBody))
            {
                _logger.LogWarning("Empty encrypted payload received for {Path}", context.Request.Path);
                await _next(context);
                return;
            }

            // Extrait le payload encrypté du JSON reçu
            // Format attendu: { "encryptedData": "base64..." }
            var encryptedPayload2 = JsonSerializer.Deserialize<EncryptedPayloadWrapper>(encryptedBody, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (encryptedPayload2 == null || string.IsNullOrWhiteSpace(encryptedPayload2.EncryptedData))
            {
                _logger.LogWarning("Invalid encrypted payload format for {Path}", context.Request.Path);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Invalid encrypted payload format. Expected: { \"encryptedData\": \"base64...\" }"
                });
                return;
            }

            // Décrypte le payload
            var decryptedJson = encryptionService.DecryptJson(encryptedPayload2.EncryptedData);

            // Remplace le body de la requête par le JSON décrypté
            var decryptedBytes = Encoding.UTF8.GetBytes(decryptedJson);
            var decryptedStream = new MemoryStream(decryptedBytes);

            // Remplace le body
            context.Request.Body = decryptedStream;
            context.Request.ContentLength = decryptedBytes.Length;
            context.Request.ContentType = "application/json";

            _logger.LogDebug("Successfully decrypted payload for {Path} (size: {Size} bytes)",
                context.Request.Path, decryptedBytes.Length);

            await _next(context);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to decrypt payload for {Path}", context.Request.Path);

            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Failed to decrypt payload",
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during payload decryption for {Path}", context.Request.Path);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Internal server error during decryption"
            });
        }
    }

    /// <summary>
    /// Wrapper pour le payload encrypté reçu du client
    /// </summary>
    private class EncryptedPayloadWrapper
    {
        public string EncryptedData { get; set; } = string.Empty;
    }
}
