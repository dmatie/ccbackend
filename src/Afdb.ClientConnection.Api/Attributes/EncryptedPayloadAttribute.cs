namespace Afdb.ClientConnection.Api.Attributes;

/// <summary>
/// Indique qu'un endpoint utilise l'encryption des payloads.
/// Peut être appliqué au niveau du controller ou de l'action.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class EncryptedPayloadAttribute : Attribute
{
    /// <summary>
    /// Si true, encrypts la requête entrante
    /// </summary>
    public bool EncryptRequest { get; set; } = true;

    /// <summary>
    /// Si true, encrypts la réponse sortante
    /// </summary>
    public bool EncryptResponse { get; set; } = true;

    /// <summary>
    /// Constructeur par défaut - encrypts requête ET réponse
    /// </summary>
    public EncryptedPayloadAttribute()
    {
    }

    /// <summary>
    /// Constructeur avec contrôle fin
    /// </summary>
    public EncryptedPayloadAttribute(bool encryptRequest, bool encryptResponse)
    {
        EncryptRequest = encryptRequest;
        EncryptResponse = encryptResponse;
    }
}

/// <summary>
/// Indique qu'un endpoint ne doit PAS utiliser l'encryption des payloads.
/// Utile pour exclure certains endpoints quand l'encryption est activée globalement.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class NoEncryptionAttribute : Attribute
{
}
