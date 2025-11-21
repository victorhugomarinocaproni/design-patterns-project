using MyShop.Domain.ValueObjects;

namespace MyShop.Domain.Interfaces;

public interface IPaymentStrategy
{
    // Returns a payment result or ID
    Task<bool> ProcessPaymentAsync(decimal amount, string currencyCode);
}
