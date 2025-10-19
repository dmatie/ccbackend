using Afdb.ClientConnection.Application.DTOs;
using MediatR;
using System.Reflection.Emit;

namespace Afdb.ClientConnection.Application.Commands.AccessRequestCmd;

public sealed record CreateOtpCommand : IRequest
{
    public string Email { get; init; } = string.Empty;
    public bool IsEmailExist { get; set; } = false;
}
