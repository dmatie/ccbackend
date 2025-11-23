using Afdb.ClientConnection.Api.Attributes;
using Afdb.ClientConnection.Application.Common.Interfaces;
using Afdb.ClientConnection.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text;

namespace Afdb.ClientConnection.Api.Middleware;

/// <summary>
/// Middleware pour encrypter automatiquement les réponses sortantes
/// </summary>
public class PayloadEncryptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PayloadEncryptionMiddleware> _logger;

    public PayloadEncryptionMiddleware(
        RequestDelegate next,
        ILogger<PayloadEncryptionMiddleware> logger)
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

        // Vérifie si l'endpoint nécessite l'encryption
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        // Vérifie les attributs
        var noEncryption = endpoint.Metadata.GetMetadata<NoEncryptionAttribute>();
        var encryptedPayload = endpoint.Metadata.GetMetadata<EncryptedPayloadAttribute>();

        // Si [NoEncryption] est présent, on skip
        if (noEncryption != null)
        {
            await _next(context);
            return;
        }

        // Détermine si on doit encrypter basé sur la configuration et les attributs
        var shouldEncrypt = false;

        // 1. Vérifie la configuration globale pour ce path
        if (encryptionService.ShouldEncrypt(context.Request.Path))
        {
            shouldEncrypt = true;
        }

        // 2. Vérifie l'attribut [EncryptedPayload]
        if (encryptedPayload != null && encryptedPayload.EncryptResponse)
        {
            shouldEncrypt = true;
        }

        // Si pas d'encryption nécessaire, on skip
        if (!shouldEncrypt)
        {
            await _next(context);
            return;
        }

        // Capture la réponse originale
        var originalBodyStream = context.Response.Body;

        try
        {
            // Remplace le body stream par un MemoryStream pour capturer la réponse
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Exécute le pipeline et laisse le controller générer sa réponse
            await _next(context);

            // Vérifie si c'est une réponse de succès avec contenu JSON
            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                // Vérifie le content-type
                var contentType = context.Response.ContentType;
                if (contentType != null && contentType.Contains("application/json"))
                {
                    // Lit le body de la réponse
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseJson = await new StreamReader(responseBody).ReadToEndAsync();

                    if (!string.IsNullOrWhiteSpace(responseJson))
                    {
                        // Encrypte le JSON
                        var encryptedData = encryptionService.EncryptJson(responseJson);

                        // Crée la réponse encryptée
                        var encryptedResponse = new EncryptedPayloadWrapper
                        {
                            EncryptedData = encryptedData
                        };

                        // Sérialise la réponse encryptée
                        var encryptedJson = System.Text.Json.JsonSerializer.Serialize(encryptedResponse);
                        var encryptedBytes = Encoding.UTF8.GetBytes(encryptedJson);

                        // Réinitialise le response body
                        context.Response.Body = originalBodyStream;
                        context.Response.ContentLength = encryptedBytes.Length;

                        // Écrit la réponse encryptée
                        await context.Response.Body.WriteAsync(encryptedBytes);

                        _logger.LogDebug("Successfully encrypted response for {Path} (original: {OriginalSize} bytes, encrypted: {EncryptedSize} bytes)",
                            context.Request.Path, responseJson.Length, encryptedBytes.Length);

                        return;
                    }
                }
            }

            // Si on arrive ici, on retourne la réponse originale (pas de JSON ou erreur)
            responseBody.Seek(0, SeekOrigin.Begin);
            context.Response.Body = originalBodyStream;
            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to encrypt response for {Path}", context.Request.Path);

            // En cas d'erreur, on retourne la réponse originale
            context.Response.Body = originalBodyStream;

            // Note: À ce stade, la réponse est peut-être déjà partiellement envoyée
            // Dans un environnement de production, il faudrait gérer ça plus proprement
        }
    }

    /// <summary>
    /// Wrapper pour la réponse encryptée envoyée au client
    /// </summary>
    private class EncryptedPayloadWrapper
    {
        public string EncryptedData { get; set; } = string.Empty;
    }
}
