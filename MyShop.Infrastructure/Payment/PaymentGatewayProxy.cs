using Microsoft.Extensions.Logging;
using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Payment;

public class PaymentGatewayProxy : IPaymentGateway
{
    private readonly ExternalPaymentSimulator _realService;
    private readonly ILogger<PaymentGatewayProxy> _logger;
    private readonly Dictionary<string, string> _cache = new(); // Simple cache for idempotency

    public PaymentGatewayProxy(ExternalPaymentSimulator realService, ILogger<PaymentGatewayProxy> logger)
    {
        _realService = realService;
        _logger = logger;
    }

    public async Task<string> ProcessPaymentAsync(decimal amount, string currency)
    {
        _logger.LogInformation($"[Proxy] Intercepting payment request: {currency} {amount}");

        // Retry Logic
        int maxRetries = 3;
        int attempt = 0;

        while (attempt < maxRetries)
        {
            try
            {
                attempt++;
                var result = await _realService.ProcessPaymentAsync(amount, currency);
                _logger.LogInformation($"[Proxy] Payment successful: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"[Proxy] Payment failed (Attempt {attempt}/{maxRetries}): {ex.Message}");
                if (attempt == maxRetries)
                {
                    _logger.LogError("[Proxy] All retries failed.");
                    throw;
                }
                await Task.Delay(1000 * attempt); // Exponential backoff
            }
        }

        throw new Exception("Unreachable code");
    }
}
