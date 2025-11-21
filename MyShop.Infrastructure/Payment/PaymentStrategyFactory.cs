using Microsoft.Extensions.DependencyInjection;
using MyShop.Domain.Interfaces;
using MyShop.Infrastructure.Payment.Strategies;

namespace MyShop.Infrastructure.Payment;

public class PaymentStrategyFactory : IPaymentStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentStrategy GetStrategy(string method)
    {
        return method.ToLower() switch
        {
            "credit" => _serviceProvider.GetRequiredService<CreditCardPaymentStrategy>(),
            "debit" => _serviceProvider.GetRequiredService<DebitCardPaymentStrategy>(),
            "pix" => _serviceProvider.GetRequiredService<PixPaymentStrategy>(),
            "boleto" => _serviceProvider.GetRequiredService<BoletoPaymentStrategy>(),
            _ => throw new ArgumentException("Invalid payment method")
        };
    }
}
