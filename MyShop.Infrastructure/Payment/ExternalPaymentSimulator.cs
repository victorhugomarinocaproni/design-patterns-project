using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Payment;

public class ExternalPaymentSimulator : IPaymentGateway
{
    public async Task<string> ProcessPaymentAsync(decimal amount, string currency)
    {
        // Simulate network delay
        await Task.Delay(500);

        // Simulate random failure (10% chance)
        var random = new Random();
        if (random.NextDouble() < 0.1)
        {
            throw new Exception("Payment gateway timeout");
        }

        return $"TX-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
