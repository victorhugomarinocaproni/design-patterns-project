using Microsoft.Extensions.Logging;
using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Payment.Strategies;

public class DebitCardPaymentStrategy : IPaymentStrategy
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ILogger<DebitCardPaymentStrategy> _logger;

    public DebitCardPaymentStrategy(IPaymentGateway paymentGateway, ILogger<DebitCardPaymentStrategy> logger)
    {
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    public async Task<bool> ProcessPaymentAsync(decimal amount, string currencyCode)
    {
        _logger.LogInformation("Processing Debit Card payment...");
        try
        {
            await _paymentGateway.ProcessPaymentAsync(amount, currencyCode);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
