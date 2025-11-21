namespace MyShop.Domain.ValueObjects;

public record Currency(string Code, string Symbol)
{
    // EF Core
    private Currency() : this(null!, null!) { }

    public static readonly Currency BRL = new("BRL", "R$");
    public static readonly Currency USD = new("USD", "$");
    public static readonly Currency EUR = new("EUR", "€");

    public override string ToString() => Code;
}
