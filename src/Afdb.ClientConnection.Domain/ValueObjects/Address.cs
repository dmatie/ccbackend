namespace Afdb.ClientConnection.Domain.ValueObjects;

public record Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string PostalCode { get; }
    public string Country { get; }

    public Address(string street, string city, string state, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        Street = street;
        City = city;
        State = state ?? string.Empty;
        PostalCode = postalCode ?? string.Empty;
        Country = country;
    }

    public override string ToString() =>
        $"{Street}, {City}" +
        (string.IsNullOrEmpty(State) ? "" : $", {State}") +
        (string.IsNullOrEmpty(PostalCode) ? "" : $" {PostalCode}") +
        $", {Country}";
}