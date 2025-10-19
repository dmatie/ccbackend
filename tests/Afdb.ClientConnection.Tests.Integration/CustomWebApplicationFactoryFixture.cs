namespace Afdb.ClientConnection.Tests.Integration;
public class CustomWebApplicationFactoryFixture : IDisposable
{
    public CustomWebApplicationFactory<Program> Factory { get; }

    public CustomWebApplicationFactoryFixture()
    {
        Factory = new CustomWebApplicationFactory<Program>();
    }

    public void Dispose()
    {
        Factory.Dispose();
    }
}
