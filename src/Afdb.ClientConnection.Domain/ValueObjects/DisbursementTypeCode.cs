namespace Afdb.ClientConnection.Domain.ValueObjects;

public static class DisbursementTypeCode
{
    public const string A1 = "A1";
    public const string A2 = "A2";
    public const string A3 = "A3";
    public const string B1 = "B1";

    public static readonly string[] All = { A1, A2, A3, B1 };

    public static bool IsValid(string code)
        => All.Contains(code);
}
