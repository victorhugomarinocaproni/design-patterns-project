using Microsoft.Extensions.Logging;
using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Payment.Strategies;

public class BoletoPaymentStrategy : IPaymentStrategy
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ILogger<BoletoPaymentStrategy> _logger;

    public BoletoPaymentStrategy(IPaymentGateway paymentGateway, ILogger<BoletoPaymentStrategy> logger)
    {
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    public async Task<bool> ProcessPaymentAsync(decimal amount, string currencyCode)
    {
        _logger.LogInformation("Processing Boleto payment...");
        // Logic specific to Boleto (e.g. generate barcode)
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
