using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Afdb.ClientConnection.Infrastructure.Services;

/// <summary>
/// Service pour l'encryption/decryption des payloads entre frontend et backend
/// Utilise AES-256-GCM pour une sécurité maximale
/// </summary>
public interface IPayloadEncryptionService
{
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

public class PayloadEncryptionService : IPayloadEncryptionService
{
    private readonly byte[] _key;
    private readonly ILogger<PayloadEncryptionService> _logger;

    // Format du payload encrypté: [12 bytes nonce][16 bytes tag][encrypted data]
    private const int NonceSize = 12; // 96 bits recommandé pour AES-GCM
    private const int TagSize = 16;   // 128 bits pour authentication tag

    public PayloadEncryptionService(
        IConfiguration configuration,
        ILogger<PayloadEncryptionService> logger)
    {
        _logger = logger;

        // Récupère la clé depuis la configuration
        var keyString = configuration["Encryption:PayloadKey"];

        if (string.IsNullOrEmpty(keyString))
        {
            throw new InvalidOperationException(
                "Encryption:PayloadKey is not configured. " +
                "Generate a key using: dotnet user-secrets set 'Encryption:PayloadKey' '<your-256-bit-key>'");
        }

        // La clé doit être en base64 et faire 32 bytes (256 bits)
        try
        {
            _key = Convert.FromBase64String(keyString);

            if (_key.Length != 32)
            {
                throw new InvalidOperationException(
                    $"Encryption key must be 256 bits (32 bytes). Current length: {_key.Length * 8} bits");
            }
        }
        catch (FormatException)
        {
            throw new InvalidOperationException(
                "Encryption:PayloadKey must be a valid base64 string. " +
                "Generate one using: Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))");
        }

        _logger.LogInformation("PayloadEncryptionService initialized with AES-256-GCM");
    }

    public string Encrypt<T>(T payload)
    {
        if (payload == null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        try
        {
            // Sérialise l'objet en JSON
            var json = JsonSerializer.Serialize(payload);
            return EncryptJson(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to encrypt payload of type {Type}", typeof(T).Name);
            throw new InvalidOperationException("Failed to encrypt payload", ex);
        }
    }

    public T Decrypt<T>(string encryptedPayload)
    {
        if (string.IsNullOrWhiteSpace(encryptedPayload))
        {
            throw new ArgumentException("Encrypted payload cannot be null or empty", nameof(encryptedPayload));
        }

        try
        {
            // Décrypte en JSON
            var json = DecryptJson(encryptedPayload);

            // Désérialise le JSON
            var result = JsonSerializer.Deserialize<T>(json);

            if (result == null)
            {
                throw new InvalidOperationException($"Failed to deserialize decrypted payload to type {typeof(T).Name}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to decrypt payload to type {Type}", typeof(T).Name);
            throw new InvalidOperationException("Failed to decrypt payload", ex);
        }
    }

    public string EncryptJson(string jsonPayload)
    {
        if (string.IsNullOrWhiteSpace(jsonPayload))
        {
            throw new ArgumentException("JSON payload cannot be null or empty", nameof(jsonPayload));
        }

        try
        {
            // Convertit le JSON en bytes
            var plainBytes = Encoding.UTF8.GetBytes(jsonPayload);

            // Génère un nonce aléatoire (doit être unique pour chaque encryption)
            var nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce);

            // Prépare le buffer pour les données encryptées + tag
            var cipherBytes = new byte[plainBytes.Length];
            var tag = new byte[TagSize];

            // Encrypte avec AES-GCM
            using var aesGcm = new AesGcm(_key, TagSize);
            aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);

            // Combine: [nonce][tag][cipher] dans un seul array
            var result = new byte[NonceSize + TagSize + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, NonceSize);
            Buffer.BlockCopy(tag, 0, result, NonceSize, TagSize);
            Buffer.BlockCopy(cipherBytes, 0, result, NonceSize + TagSize, cipherBytes.Length);

            // Retourne en base64
            return Convert.ToBase64String(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to encrypt JSON payload");
            throw new InvalidOperationException("Failed to encrypt JSON payload", ex);
        }
    }

    public string DecryptJson(string encryptedPayload)
    {
        if (string.IsNullOrWhiteSpace(encryptedPayload))
        {
            throw new ArgumentException("Encrypted payload cannot be null or empty", nameof(encryptedPayload));
        }

        try
        {
            // Décode le base64
            var encryptedBytes = Convert.FromBase64String(encryptedPayload);

            // Vérifie la taille minimale
            if (encryptedBytes.Length < NonceSize + TagSize)
            {
                throw new InvalidOperationException(
                    $"Encrypted payload is too short. Expected at least {NonceSize + TagSize} bytes, got {encryptedBytes.Length}");
            }

            // Extrait nonce, tag et cipher
            var nonce = new byte[NonceSize];
            var tag = new byte[TagSize];
            var cipherBytes = new byte[encryptedBytes.Length - NonceSize - TagSize];

            Buffer.BlockCopy(encryptedBytes, 0, nonce, 0, NonceSize);
            Buffer.BlockCopy(encryptedBytes, NonceSize, tag, 0, TagSize);
            Buffer.BlockCopy(encryptedBytes, NonceSize + TagSize, cipherBytes, 0, cipherBytes.Length);

            // Prépare le buffer pour les données décryptées
            var plainBytes = new byte[cipherBytes.Length];

            // Décrypte avec AES-GCM (vérifie aussi l'authenticité avec le tag)
            using var aesGcm = new AesGcm(_key, TagSize);
            aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);

            // Convertit en string UTF-8
            return Encoding.UTF8.GetString(plainBytes);
        }
        catch (CryptographicException ex)
        {
            _logger.LogError(ex, "Failed to decrypt payload - authentication failed or corrupted data");
            throw new InvalidOperationException("Failed to decrypt payload - data may be corrupted or tampered", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to decrypt payload");
            throw new InvalidOperationException("Failed to decrypt payload", ex);
        }
    }
}
