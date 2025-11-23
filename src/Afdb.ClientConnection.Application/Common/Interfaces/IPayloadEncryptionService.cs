namespace Afdb.ClientConnection.Application.Common.Interfaces;

/// <summary>
/// Service pour l'encryption/decryption des payloads entre frontend et backend
/// Utilise AES-256-GCM pour une sécurité maximale
/// </summary>
public interface IPayloadEncryptionService
{
    /// <summary>
    /// Vérifie si l'encryption est activée globalement
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Vérifie si un endpoint spécifique doit être encrypté
    /// </summary>
    bool ShouldEncrypt(string path);

    /// <summary>
    /// Encrypts a payload object to a base64 string
    /// </summary>
    string Encrypt<T>(T payload);

    /// <summary>
    /// Decrypts a base64 string to a payload object
    /// </summary>
    T Decrypt<T>(string encryptedPayload);

    /// <summary>
    /// Encrypts raw JSON string
    /// </summary>
    string EncryptJson(string jsonPayload);

    /// <summary>
    /// Decrypts to raw JSON string
    /// </summary>
    string DecryptJson(string encryptedPayload);
}