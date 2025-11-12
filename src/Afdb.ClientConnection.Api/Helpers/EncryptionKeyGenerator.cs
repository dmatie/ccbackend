using System.Security.Cryptography;

namespace Afdb.ClientConnection.Api.Helpers;

/// <summary>
/// Helper pour générer des clés d'encryption sécurisées
/// </summary>
public static class EncryptionKeyGenerator
{
    /// <summary>
    /// Génère une clé AES-256 aléatoire (32 bytes) en base64
    /// </summary>
    /// <returns>Clé en format base64</returns>
    public static string GenerateKey()
    {
        var key = RandomNumberGenerator.GetBytes(32); // 256 bits
        return Convert.ToBase64String(key);
    }

    /// <summary>
    /// Point d'entrée pour générer et afficher une clé
    /// Usage: Ajouter un endpoint temporaire ou utiliser en console
    /// </summary>
    public static void PrintNewKey()
    {
        var key = GenerateKey();
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine("GENERATED AES-256 ENCRYPTION KEY");
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine();
        Console.WriteLine("⚠️  IMPORTANT: Store this key securely!");
        Console.WriteLine();
        Console.WriteLine("Key (base64):");
        Console.WriteLine(key);
        Console.WriteLine();
        Console.WriteLine("To use in appsettings.json (NOT RECOMMENDED for production):");
        Console.WriteLine($"\"Encryption\": {{ \"PayloadKey\": \"{key}\" }}");
        Console.WriteLine();
        Console.WriteLine("To use with User Secrets (RECOMMENDED for development):");
        Console.WriteLine($"dotnet user-secrets set \"Encryption:PayloadKey\" \"{key}\"");
        Console.WriteLine();
        Console.WriteLine("To use with Azure Key Vault (RECOMMENDED for production):");
        Console.WriteLine("1. Store in Key Vault as a secret named 'EncryptionPayloadKey'");
        Console.WriteLine("2. Configure: \"Encryption\": { \"PayloadKey\": \"@Microsoft.KeyVault(SecretUri=...)\" }");
        Console.WriteLine();
        Console.WriteLine("=".PadRight(80, '='));
    }
}
