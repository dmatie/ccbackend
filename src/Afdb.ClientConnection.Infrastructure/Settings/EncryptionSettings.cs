namespace Afdb.ClientConnection.Infrastructure.Settings;

/// <summary>
/// Configuration pour l'encryption des payloads
/// </summary>
public class EncryptionSettings
{
    public const string SectionName = "Encryption";

    /// <summary>
    /// Active/désactive l'encryption globalement
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// Clé d'encryption AES-256 (32 bytes en base64)
    /// </summary>
    public string PayloadKey { get; set; } = string.Empty;

    /// <summary>
    /// Liste des endpoints qui doivent TOUJOURS être encryptés (même si Enabled = false)
    /// Utile pour forcer l'encryption sur des endpoints sensibles
    /// </summary>
    public List<string> AlwaysEncryptEndpoints { get; set; } = new();

    /// <summary>
    /// Liste des endpoints qui ne doivent JAMAIS être encryptés (même si Enabled = true)
    /// Utile pour exclure des endpoints publics comme /health, /swagger
    /// </summary>
    public List<string> NeverEncryptEndpoints { get; set; } = new()
    {
        "/health",
        "/swagger",
        "/_blazor"
    };

    /// <summary>
    /// Mode de fonctionnement de l'encryption
    /// - Global: Tous les endpoints sont encryptés si Enabled = true
    /// - Attribute: Seuls les endpoints avec [EncryptedPayload] sont encryptés
    /// </summary>
    public EncryptionMode Mode { get; set; } = EncryptionMode.Attribute;
}

/// <summary>
/// Mode de fonctionnement de l'encryption
/// </summary>
public enum EncryptionMode
{
    /// <summary>
    /// Encryption activée globalement sur tous les endpoints (sauf NeverEncryptEndpoints)
    /// </summary>
    Global,

    /// <summary>
    /// Encryption activée uniquement sur les endpoints avec [EncryptedPayload]
    /// </summary>
    Attribute
}
