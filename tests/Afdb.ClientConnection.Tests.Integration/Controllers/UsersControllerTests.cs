using Afdb.ClientConnection.Application.Commands.UserCmd;
using Afdb.ClientConnection.Domain.Enums;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Afdb.ClientConnection.Tests.Integration.Controllers;

[Collection("IntegrationTestCollection")]
public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UsersControllerTests(CustomWebApplicationFactoryFixture fixture)
    {
        _client = fixture.Factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "newuser@example.com",
            FirstName = "John",
            LastName = "Doe",
            Role = UserRole.DA,
            EntraIdObjectId = "entra-123",
            OrganizationName = "Test Organization"
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant

        // Act

        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CreateUserResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.User.Id);
        Assert.Equal(command.Email, result.User.Email);
        Assert.Equal(command.Role, result.User.Role);
    }

    [Fact]
    public async Task CreateUser_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "invalid-email", // Invalid email format
            FirstName = "",          // Empty first name
            LastName = "Doe",
            Role = UserRole.Admin
        };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant


        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetUserByEmail_WithValidEmail_ReturnsOkResult()
    {
        // Arrange - First create a user
        var createCommand = new CreateUserCommand
        {
            Email = "search@example.com",
            FirstName = "Search",
            LastName = "User",
            Role = UserRole.DO,
            EntraIdObjectId = "entra-search"
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant

        var createResponse = await _client.PostAsJsonAsync("/api/users", createCommand);
        Assert.Equal(System.Net.HttpStatusCode.Created, createResponse.StatusCode);

        // Act
        var response = await _client.GetAsync($"/api/users/by-email/{createCommand.Email}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Search", responseContent);
        Assert.Contains("User", responseContent);
    }

    [Fact]
    public async Task GetUserByEmail_WithNonExistentEmail_ReturnsNotFound()
    {
        // Arrange
        var nonExistentEmail = "nonexistent@example.com";
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant

        // Act
        var response = await _client.GetAsync($"/api/users/by-email/{nonExistentEmail}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetUserByEntraId_WithValidId_ReturnsOkResult()
    {
        // Arrange - First create a user
        var createCommand = new CreateUserCommand
        {
            Email = "entraid@example.com",
            FirstName = "EntraId",
            LastName = "User",
            Role = UserRole.Admin,
            EntraIdObjectId = "entra-unique-123"
        };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant

        var createResponse = await _client.PostAsJsonAsync("/api/users", createCommand);
        Assert.Equal(System.Net.HttpStatusCode.Created, createResponse.StatusCode);

        // Act
        var response = await _client.GetAsync($"/api/users/by-entraid/{createCommand.EntraIdObjectId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("EntraId", responseContent);
        Assert.Contains("User", responseContent);
    }

    [Fact]
    public async Task GetUserByEntraId_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentEntraId = "non-existent-entra-id";
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant

        // Act
        var response = await _client.GetAsync($"/api/users/by-entraid/{nonExistentEntraId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_AsAdmin_ReturnsOkResult()
    {
        // Arrange - Create some test users first
        var user1 = new CreateUserCommand
        {
            Email = "list1@example.com",
            FirstName = "List1",
            LastName = "User",
            Role = UserRole.DA,
            EntraIdObjectId = "entra-list1"
        };

        var user2 = new CreateUserCommand
        {
            Email = "list2@example.com",
            FirstName = "List2",
            LastName = "User",
            Role = UserRole.DO,
            EntraIdObjectId = "entra-list2"
        };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin"); // rôle insuffisant


        await _client.PostAsJsonAsync("/api/users", user1);
        await _client.PostAsJsonAsync("/api/users", user2);

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("List1", responseContent);
        Assert.Contains("List2", responseContent);
    }
}
