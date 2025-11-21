using Microsoft.Extensions.Logging;
using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;

namespace MyShop.Infrastructure.Observers;

public class CartInventoryObserver : ICartObserver
{
    private readonly ILogger<CartInventoryObserver> _logger;

    public CartInventoryObserver(ILogger<CartInventoryObserver> logger)
    {
        _logger = logger;
    }

    public void OnItemAdded(CartItem item)
    {
        _logger.LogInformation($"[Inventory] Item added to cart. Reserving stock for Product {item.ProductName}, Quantity: {item.Quantity}");
    }

    public void OnItemRemoved(CartItem item)
    {
        _logger.LogInformation($"[Inventory] Item removed from cart. Releasing stock for Product {item.ProductName}, Quantity: {item.Quantity}");
    }

    public void OnCartCleared()
    {
        _logger.LogInformation("[Inventory] Cart cleared. Releasing all reserved stock.");
    }
}
