using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

namespace Afdb.ClientConnection.Tests.Unit.Application.Commands;

public class CreateAccessRequestCommandValidatorTests
{
    private readonly CreateAccessRequestCommandValidator _validator;

    public CreateAccessRequestCommandValidatorTests()
    {
        _validator = new CreateAccessRequestCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var command = new CreateAccessRequestCommand { Email = "" };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Email));
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var command = new CreateAccessRequestCommand { Email = "invalid-email" };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Email));
    }

    [Fact]
    public void Should_Have_Error_When_Email_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateAccessRequestCommand { Email = new string('a', 250) + "@test.com" };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Email));
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        // Arrange
        var command = new CreateAccessRequestCommand { FirstName = "" };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.FirstName));
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateAccessRequestCommand { FirstName = new string('a', 101) };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.FirstName));
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        // Arrange
        var command = new CreateAccessRequestCommand { LastName = "" };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.LastName));
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateAccessRequestCommand { LastName = new string('a', 101) };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.LastName));
    }

    [Fact]
    public void Should_Have_Error_When_FunctionId_Is_Empty_Guid()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            FunctionId = Guid.Empty
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.FunctionId));
    }

    [Fact]
    public void Should_Have_Error_When_CountryId_Is_Empty_Guid()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CountryId = Guid.Empty
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.CountryId));
    }

    [Fact]
    public void Should_Have_Error_When_BusinessProfileId_Is_Empty_Guid()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            BusinessProfileId = Guid.Empty
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.BusinessProfileId));
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid_With_All_Ids()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            FunctionId = Guid.NewGuid(),
            CountryId = Guid.NewGuid(),
            BusinessProfileId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid_Without_Optional_Ids()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
            // FunctionId, CountryId, BusinessProfileId sont null (optionnels)
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Only_FunctionId_Is_Provided()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            FunctionId = Guid.NewGuid()
            // CountryId et BusinessProfileId sont null
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Only_CountryId_Is_Provided()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            CountryId = Guid.NewGuid()
            // FunctionId et BusinessProfileId sont null
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Only_BusinessProfileId_Is_Provided()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            BusinessProfileId = Guid.NewGuid()
            // FunctionId et CountryId sont null
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Too_Long()
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = new string('a', 256) + "@test.com", // 256 + 9 = 265 caractères
            FirstName = "John",
            LastName = "Doe"
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Email));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Should_Have_Error_When_Email_Is_Empty_Or_Whitespace(string email)
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = email,
            FirstName = "John",
            LastName = "Doe"
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Email));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Should_Have_Error_When_FirstName_Is_Empty_Or_Whitespace(string firstName)
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = firstName,
            LastName = "Doe"
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.FirstName));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Should_Have_Error_When_LastName_Is_Empty_Or_Whitespace(string lastName)
    {
        // Arrange
        var command = new CreateAccessRequestCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = lastName
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.LastName));
    }
}