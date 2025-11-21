using Microsoft.Extensions.Logging;
using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Payment.Strategies;

public class CreditCardPaymentStrategy : IPaymentStrategy
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ILogger<CreditCardPaymentStrategy> _logger;

    public CreditCardPaymentStrategy(IPaymentGateway paymentGateway, ILogger<CreditCardPaymentStrategy> logger)
    {
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    public async Task<bool> ProcessPaymentAsync(decimal amount, string currencyCode)
    {
        _logger.LogInformation("Processing Credit Card payment...");
        // In a real app, we would validate card details here
        try
        {
            await _paymentGateway.ProcessPaymentAsync(amount, currencyCode);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
