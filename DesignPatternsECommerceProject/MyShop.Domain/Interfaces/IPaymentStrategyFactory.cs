using MyShop.Domain.Interfaces;

namespace MyShop.Domain.Interfaces;

public interface IPaymentStrategyFactory
{
    IPaymentStrategy GetStrategy(string method);
}
