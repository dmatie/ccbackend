namespace Afdb.ClientConnection.Application.DTOs;

public sealed record ProjectLoanNumberDto
{
    public string SapCode { get; init; } = default!;
    public string LoanNumber { get; init; } = default!;
}
