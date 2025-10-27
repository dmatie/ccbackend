using Afdb.ClientConnection.Domain.Common;
using Afdb.ClientConnection.Domain.EntitiesParams;

namespace Afdb.ClientConnection.Domain.Entities;

public sealed class Currency : BaseEntity
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string Symbol { get; private set; }

    private Currency() { }

    public Currency(CurrencyLoadParam loadParam )
    {
        if (string.IsNullOrWhiteSpace(loadParam.Code))
            throw new ArgumentException("Code cannot be empty");

        if (string.IsNullOrWhiteSpace(loadParam.Name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(loadParam.Symbol))
            throw new ArgumentException("Symbol cannot be empty");

        Id= loadParam.Id;
        Code = loadParam.Code.ToUpper();
        Name = loadParam.Name;
        Symbol = loadParam.Symbol;
        CreatedBy = loadParam.CreatedBy;
        CreatedAt = loadParam.CreatedAt;
        UpdatedAt = loadParam.UpdatedAt;
        UpdatedBy = loadParam.UpdatedBy;
    }
}
