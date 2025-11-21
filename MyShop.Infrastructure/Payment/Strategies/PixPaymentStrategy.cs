using Microsoft.Extensions.Logging;
using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Payment.Strategies;

public class PixPaymentStrategy : IPaymentStrategy
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ILogger<PixPaymentStrategy> _logger;

    public PixPaymentStrategy(IPaymentGateway paymentGateway, ILogger<PixPaymentStrategy> logger)
    {
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    public async Task<bool> ProcessPaymentAsync(decimal amount, string currencyCode)
    {
        _logger.LogInformation("Processing PIX payment...");
        // Logic specific to PIX (e.g. generate QR Code)
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
