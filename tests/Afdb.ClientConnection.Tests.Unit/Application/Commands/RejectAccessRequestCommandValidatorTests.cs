using Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

namespace Afdb.ClientConnection.Tests.Unit.Application.Commands;

public class RejectAccessRequestCommandValidatorTests
{
    private readonly RejectAccessRequestCommandValidator _validator;

    public RejectAccessRequestCommandValidatorTests()
    {
        _validator = new RejectAccessRequestCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_AccessRequestId_Is_Empty()
    {
        // Arrange
        var command = new RejectAccessRequestCommand { AccessRequestId = Guid.Empty };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.AccessRequestId));
    }

    [Fact]
    public void Should_Have_Error_When_RejectionReason_Is_Empty()
    {
        // Arrange
        var command = new RejectAccessRequestCommand { RejectionReason = "" };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.RejectionReason));
    }

    [Fact]
    public void Should_Have_Error_When_RejectionReason_Is_Too_Short()
    {
        // Arrange
        var command = new RejectAccessRequestCommand { RejectionReason = "Short" };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.RejectionReason));
    }

    [Fact]
    public void Should_Have_Error_When_RejectionReason_Exceeds_MaxLength()
    {
        // Arrange
        var command = new RejectAccessRequestCommand { RejectionReason = new string('a', 1001) };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.RejectionReason));
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new RejectAccessRequestCommand
        {
            AccessRequestId = Guid.NewGuid(),
            RejectionReason = "Insufficient documentation provided for verification"
        };

        // Act & Assert
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}