using MediatR;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record VerifyOtpCommand : IRequest<bool>
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
}
