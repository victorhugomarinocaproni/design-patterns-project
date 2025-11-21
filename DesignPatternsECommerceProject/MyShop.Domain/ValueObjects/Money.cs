using MyShop.Domain.Exceptions;

namespace MyShop.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new DomainException("Money amount cannot be negative");
            
        Amount = amount;
        Currency = currency;
    }

    private Money() { }

    public static Money Zero(Currency currency) => new(0, currency);

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new DomainException("Cannot add money of different currencies");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new DomainException("Cannot subtract money of different currencies");
        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public static Money operator *(Money a, int multiplier)
    {
        return new Money(a.Amount * multiplier, a.Currency);
    }
    
    public override string ToString() => $"{Currency.Symbol} {Amount:F2}";
}
