using Xunit;

namespace Afdb.ClientConnection.Tests.Integration;

[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestCollection : ICollectionFixture<CustomWebApplicationFactoryFixture>
{
    // Pas de code ici, juste la déclaration
}