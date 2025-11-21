using Microsoft.Extensions.Logging;
using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Observers;

public class CartAnalyticsObserver : ICartObserver
{
    private readonly ILogger<CartAnalyticsObserver> _logger;

    public CartAnalyticsObserver(ILogger<CartAnalyticsObserver> logger)
    {
        _logger = logger;
    }

    public void OnItemAdded(CartItem item)
    {
        _logger.LogInformation($"[Analytics] User added {item.Quantity}x {item.ProductName} to cart.");
    }

    public void OnItemRemoved(CartItem item)
    {
        _logger.LogInformation($"[Analytics] User removed {item.ProductName} from cart.");
    }

    public void OnCartCleared()
    {
        _logger.LogInformation("[Analytics] User abandoned/cleared cart.");
    }
}
