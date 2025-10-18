using Afdb.ClientConnection.Domain.Entities;
using Afdb.ClientConnection.Domain.EntitiesParams;
using Afdb.ClientConnection.Domain.Enums;
using Afdb.ClientConnection.Domain.Events;

namespace Afdb.ClientConnection.Tests.Unit.Domain.Entities;

public sealed class AccessRequestTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesAccessRequest()
    {
        // Arrange
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";
        var createdBy = "system";
        var functionId = Guid.NewGuid();
        var countryId = Guid.NewGuid();
        var businessProfileId = Guid.NewGuid();

        // Act
        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedBy = createdBy,
            FunctionId = functionId,
            CountryId = countryId,
            BusinessProfileId = businessProfileId
        });

        // Assert
        Assert.Equal(email.ToLowerInvariant(), accessRequest.Email);
        Assert.Equal(firstName, accessRequest.FirstName);
        Assert.Equal(lastName, accessRequest.LastName);
        Assert.Equal(functionId, accessRequest.FunctionId);
        Assert.Equal(countryId, accessRequest.CountryId);
        Assert.Equal(businessProfileId, accessRequest.BusinessProfileId);
        Assert.Equal(RequestStatus.Pending, accessRequest.Status);
        Assert.Equal(createdBy, accessRequest.CreatedBy);
        Assert.True(accessRequest.CanBeProcessed);
        Assert.False(accessRequest.IsProcessed);
        Assert.Single(accessRequest.DomainEvents);
        Assert.IsType<AccessRequestCreatedEvent>(accessRequest.DomainEvents.First());
    }

    [Fact]
    public void Constructor_WithValidDataAndEntities_CreatesAccessRequestWithNavigationProperties()
    {
        // Arrange
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";
        var createdBy = "system";
        var functionId = Guid.NewGuid();
        var countryId = Guid.NewGuid();
        var businessProfileId = Guid.NewGuid();

        var function = new Function(functionId, "ADB Desk Office","ADBDESK" ,"African Development Bank Desk Office", createdBy);
        var country = new Country(countryId, "Algeria", "Algérie", "DZA", createdBy);
        var businessProfile = new BusinessProfile(businessProfileId, "Executing Agency", "Executing Agency", createdBy);

        // Act
        var accessRequest = new AccessRequest(new AccessRequestNewParam()
        {
            Email= email,
            FirstName= firstName,
            LastName= lastName,
            CreatedBy= createdBy,
            FunctionId= functionId,
            CountryId= countryId,
            BusinessProfileId= businessProfileId,
            Function= function,
            Country= country,
            BusinessProfile= businessProfile,
        });

        // Assert
        Assert.Equal(email.ToLowerInvariant(), accessRequest.Email);
        Assert.Equal(firstName, accessRequest.FirstName);
        Assert.Equal(lastName, accessRequest.LastName);
        Assert.Equal(functionId, accessRequest.FunctionId);
        Assert.Equal(countryId, accessRequest.CountryId);
        Assert.Equal(businessProfileId, accessRequest.BusinessProfileId);
        Assert.Equal(function, accessRequest.Function);
        Assert.Equal(country, accessRequest.Country);
        Assert.Equal(businessProfile, accessRequest.BusinessProfile);
        Assert.Equal(RequestStatus.Pending, accessRequest.Status);
        Assert.Equal(createdBy, accessRequest.CreatedBy);

        // Vérifier que l'event contient les bonnes valeurs
        var createdEvent = accessRequest.DomainEvents.OfType<AccessRequestCreatedEvent>().First();
        Assert.Equal("ADB Desk Office", createdEvent.Function);
        Assert.Equal("Executing Agency", createdEvent.BusinessProfile);
        Assert.Equal("Algeria", createdEvent.Country);
    }

    [Theory]
    [InlineData("", "John", "Doe")]
    [InlineData("test@example.com", "", "Doe")]
    [InlineData("test@example.com", "John", "")]
    public void Constructor_WithInvalidData_ThrowsArgumentException(
        string email, string firstName, string lastName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new AccessRequest(new AccessRequestNewParam()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CreatedBy = "System"
            }));
    }

    [Fact]
    public void Approve_WithValidData_UpdatesStatusAndAddsEvent()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var processedById = Guid.NewGuid();
        var comments = "Approved for access";
        var updatedBy = "approver";

        // Act
        accessRequest.Approve(processedById, comments, updatedBy,true);

        // Assert
        Assert.Equal(RequestStatus.Approved, accessRequest.Status);
        Assert.Equal(processedById, accessRequest.ProcessedById);
        Assert.Equal(comments, accessRequest.ProcessingComments);
        Assert.NotNull(accessRequest.ProcessedDate);
        Assert.False(accessRequest.CanBeProcessed);
        Assert.True(accessRequest.IsProcessed);
        Assert.Contains(accessRequest.DomainEvents, e => e is AccessRequestApprovedEvent);
    }

    [Fact]
    public void Approve_WhenNotPending_ThrowsInvalidOperationException()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        accessRequest.Approve(Guid.NewGuid(), "First approval", "user1",true);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            accessRequest.Approve(Guid.NewGuid(), "Second approval", "user2",true));
        Assert.Contains("Only pending requests can be approved", exception.Message);
    }

    [Fact]
    public void Reject_WithValidData_UpdatesStatusAndAddsEvent()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var processedById = Guid.NewGuid();
        var rejectionReason = "Insufficient documentation";
        var updatedBy = "approver";

        // Act
        accessRequest.Reject(processedById, rejectionReason, updatedBy, true);

        // Assert
        Assert.Equal(RequestStatus.Rejected, accessRequest.Status);
        Assert.Equal(processedById, accessRequest.ProcessedById);
        Assert.Equal(rejectionReason, accessRequest.ProcessingComments);
        Assert.NotNull(accessRequest.ProcessedDate);
        Assert.False(accessRequest.CanBeProcessed);
        Assert.True(accessRequest.IsProcessed);
        Assert.Contains(accessRequest.DomainEvents, e => e is AccessRequestRejectedEvent);
    }

    [Fact]
    public void Reject_WithEmptyReason_ThrowsArgumentException()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            accessRequest.Reject(Guid.NewGuid(), "", "user", true));
        Assert.Contains("Rejection reason is required", exception.Message);
    }

    [Fact]
    public void SetEntraIdObjectId_WhenApproved_UpdatesObjectId()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        accessRequest.Approve(Guid.NewGuid(), "Approved", "user", true);
        var entraIdObjectId = "entra-id-123";

        // Act
        accessRequest.SetEntraIdObjectId(entraIdObjectId, "system");

        // Assert
        Assert.Equal(entraIdObjectId, accessRequest.EntraIdObjectId);
        Assert.True(accessRequest.HasEntraIdAccount);
    }

    [Fact]
    public void SetEntraIdObjectId_WhenNotApproved_ThrowsInvalidOperationException()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            accessRequest.SetEntraIdObjectId("entra-id-123", "system"));
        Assert.Contains("Can only set Entra ID for approved requests", exception.Message);
    }

    [Fact]
    public void FullName_ReturnsCorrectFormat()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();

        // Act
        var fullName = accessRequest.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void UpdateFunction_WithValidIdAndEntity_UpdatesFunctionIdAndEntity()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var newFunctionId = Guid.NewGuid();
        var newFunction = new Function(newFunctionId, "New Function","NF", "New Function Description", "system");
        var updatedBy = "user";

        // Act
        accessRequest.UpdateFunction(newFunctionId, newFunction, updatedBy);

        // Assert
        Assert.Equal(newFunctionId, accessRequest.FunctionId);
        Assert.Equal(newFunction, accessRequest.Function);
    }

    [Fact]
    public void UpdateFunction_WithNullEntity_UpdatesFunctionIdOnly()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var newFunctionId = Guid.NewGuid();
        var updatedBy = "user";

        // Act
        accessRequest.UpdateFunction(newFunctionId, null, updatedBy);

        // Assert
        Assert.Equal(newFunctionId, accessRequest.FunctionId);
        Assert.Null(accessRequest.Function);
    }

    [Fact]
    public void UpdateCountry_WithValidIdAndEntity_UpdatesCountryIdAndEntity()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var newCountryId = Guid.NewGuid();
        var newCountry = new Country(newCountryId, "Morocco", "Maroc", "MAR", "system");
        var updatedBy = "user";

        // Act
        accessRequest.UpdateCountry(newCountryId, newCountry, updatedBy);

        // Assert
        Assert.Equal(newCountryId, accessRequest.CountryId);
        Assert.Equal(newCountry, accessRequest.Country);
    }

    [Fact]
    public void UpdateCountry_WithNullEntity_UpdatesCountryIdOnly()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var newCountryId = Guid.NewGuid();
        var updatedBy = "user";

        // Act
        accessRequest.UpdateCountry(newCountryId, null, updatedBy);

        // Assert
        Assert.Equal(newCountryId, accessRequest.CountryId);
        Assert.Null(accessRequest.Country);
    }

    [Fact]
    public void UpdateBusinessProfile_WithValidIdAndEntity_UpdatesBusinessProfileIdAndEntity()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var newBusinessProfileId = Guid.NewGuid();
        var newBusinessProfile = new BusinessProfile(newBusinessProfileId, "New Profile", "New Profile Description", "system");
        var updatedBy = "user";

        // Act
        accessRequest.UpdateBusinessProfile(newBusinessProfileId, newBusinessProfile, updatedBy);

        // Assert
        Assert.Equal(newBusinessProfileId, accessRequest.BusinessProfileId);
        Assert.Equal(newBusinessProfile, accessRequest.BusinessProfile);
    }

    [Fact]
    public void UpdateBusinessProfile_WithNullEntity_UpdatesBusinessProfileIdOnly()
    {
        // Arrange
        var accessRequest = CreateTestAccessRequest();
        var newBusinessProfileId = Guid.NewGuid();
        var updatedBy = "user";

        // Act
        accessRequest.UpdateBusinessProfile(newBusinessProfileId, null, updatedBy);

        // Assert
        Assert.Equal(newBusinessProfileId, accessRequest.BusinessProfileId);
        Assert.Null(accessRequest.BusinessProfile);
    }

    [Fact]
    public void Constructor_WithOptionalIds_CreatesAccessRequestWithNulls()
    {
        // Arrange
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";
        var createdBy = "system";

        // Act
        var accessRequest = new AccessRequest(new AccessRequestNewParam() 
        { 
            Email=email,
            FirstName=firstName,
            LastName=lastName,
            CreatedBy=createdBy,
        });

        // Assert
        Assert.Equal(email.ToLowerInvariant(), accessRequest.Email);
        Assert.Equal(firstName, accessRequest.FirstName);
        Assert.Equal(lastName, accessRequest.LastName);
        Assert.Null(accessRequest.FunctionId);
        Assert.Null(accessRequest.CountryId);
        Assert.Null(accessRequest.BusinessProfileId);
        Assert.Null(accessRequest.Function);
        Assert.Null(accessRequest.Country);
        Assert.Null(accessRequest.BusinessProfile);
        Assert.Equal(RequestStatus.Pending, accessRequest.Status);
        Assert.Equal(createdBy, accessRequest.CreatedBy);

        // Vérifier que l'event contient des valeurs null
        var createdEvent = accessRequest.DomainEvents.OfType<AccessRequestCreatedEvent>().First();
        Assert.Null(createdEvent.Function);
        Assert.Null(createdEvent.BusinessProfile);
        Assert.Null(createdEvent.Country);
    }

    private static AccessRequest CreateTestAccessRequest()
    {
        return new AccessRequest(new AccessRequestNewParam() 
        {
            Email= "test@example.com",
            FirstName= "John",
            LastName="Doe",
            CreatedBy= "system"
        });
    }
}