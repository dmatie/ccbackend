using Afdb.ClientConnection.Domain.Common;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class Currency : BaseEntity
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string Symbol { get; private set; }

    private Currency() { }

    public Currency(string code, string name, string symbol)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol cannot be empty");

        Code = code.ToUpper();
        Name = name;
        Symbol = symbol;
        CreatedBy = "System";
    }
}
