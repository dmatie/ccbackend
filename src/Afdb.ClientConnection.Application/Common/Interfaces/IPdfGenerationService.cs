namespace Afdb.ClientConnection.Application.Common.Interfaces;

public interface IPdfGenerationService
{
    byte[] GenerateAuthorizationForm(
        string firstName,
        string lastName,
        string email,
        string functionName,
        List<string> projects,
        string language);
}