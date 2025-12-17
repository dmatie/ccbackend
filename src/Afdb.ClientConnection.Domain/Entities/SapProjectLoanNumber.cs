namespace Afdb.ClientConnection.Domain.Entities;

public sealed class SapProjectLoanNumber
{
    public string ProjectCode { get; set; } = default!;
    public string LoanNumber { get; set; } = default!;
}
