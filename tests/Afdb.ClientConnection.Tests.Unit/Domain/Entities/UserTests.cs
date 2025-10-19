using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.Enums;

namespace Afdb.ClientConnection.Tests.Unit.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesUser()
    {
        // Arrange
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";
        var role = UserRole.DO;
        var entraIdObjectId = "entra-123";
        var createdBy = "admin";
        var organizationName = "Test Org";

        // Act
        var user = new User(email, firstName, lastName, role, entraIdObjectId, createdBy, organizationName);

        // Assert
        Assert.Equal(email.ToLowerInvariant(), user.Email);
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(role, user.Role);
        Assert.Equal(entraIdObjectId, user.EntraIdObjectId);
        Assert.Equal(organizationName, user.OrganizationName);
        Assert.True(user.IsActive);
        Assert.True(user.IsInternal);
        Assert.False(user.IsExternal);
        Assert.True(user.HasEntraIdAccount);
    }

    [Fact]
    public void CreateExternalUser_CreatesUserWithExternalRole()
    {
        // Arrange
        var email = "external@company.com";
        var firstName = "Jane";
        var lastName = "Smith";
        var organizationName = "External Company";
        var entraIdObjectId = "entra-456";
        var createdBy = "system";

        // Act
        var user = User.CreateExternalUser(email, firstName, lastName, organizationName, entraIdObjectId, createdBy);

        // Assert
        Assert.Equal(UserRole.ExternalUser, user.Role);
        Assert.Equal(organizationName, user.OrganizationName);
        Assert.False(user.IsInternal);
        Assert.True(user.IsExternal);
    }

    [Theory]
    [InlineData("", "John", "Doe")]
    [InlineData("test@example.com", "", "Doe")]
    [InlineData("test@example.com", "John", "")]
    public void Constructor_WithInvalidData_ThrowsArgumentException(string email, string firstName, string lastName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new User(email, firstName, lastName, UserRole.DO, "entra-123", "admin"));
    }

    [Fact]
    public void UpdateProfile_UpdatesUserInformation()
    {
        // Arrange
        var user = CreateTestUser();
        var newFirstName = "Jane";
        var newLastName = "Smith";
        var newOrganizationName = "New Organization";
        var updatedBy = "admin";

        // Act
        user.UpdateProfile(newFirstName, newLastName, newOrganizationName, updatedBy);

        // Assert
        Assert.Equal(newFirstName, user.FirstName);
        Assert.Equal(newLastName, user.LastName);
        Assert.Equal(newOrganizationName, user.OrganizationName);
        Assert.NotNull(user.UpdatedAt);
        Assert.Equal(updatedBy, user.UpdatedBy);
    }

    [Fact]
    public void ChangeRole_UpdatesUserRole()
    {
        // Arrange
        var user = CreateTestUser();
        var newRole = UserRole.Admin;
        var updatedBy = "admin";

        // Act
        user.ChangeRole(newRole, updatedBy);

        // Assert
        Assert.Equal(newRole, user.Role);
        Assert.NotNull(user.UpdatedAt);
        Assert.Equal(updatedBy, user.UpdatedBy);
    }

    [Fact]
    public void ChangeRole_WithSameRole_DoesNotUpdate()
    {
        // Arrange
        var user = CreateTestUser();
        var originalUpdatedAt = user.UpdatedAt;

        // Act
        user.ChangeRole(user.Role, "admin");

        // Assert
        Assert.Equal(originalUpdatedAt, user.UpdatedAt);
    }

    [Fact]
    public void Deactivate_SetsIsActiveToFalse()
    {
        // Arrange
        var user = CreateTestUser();
        var updatedBy = "admin";

        // Act
        user.Deactivate(updatedBy);

        // Assert
        Assert.False(user.IsActive);
        Assert.NotNull(user.UpdatedAt);
        Assert.Equal(updatedBy, user.UpdatedBy);
    }

    [Fact]
    public void Activate_SetsIsActiveToTrue()
    {
        // Arrange
        var user = CreateTestUser();
        user.Deactivate("admin");

        // Act
        user.Activate("admin");

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void FullName_ReturnsCorrectFormat()
    {
        // Arrange
        var user = CreateTestUser();

        // Act
        var fullName = user.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    private static User CreateTestUser()
    {
        return new User(
            "test@example.com",
            "John",
            "Doe",
            UserRole.DO,
            "entra-123",
            "admin",
            "Test Organization");
    }
}
