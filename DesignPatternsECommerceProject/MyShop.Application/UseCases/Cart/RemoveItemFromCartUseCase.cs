using MyShop.Domain.Interfaces;

namespace MyShop.Application.UseCases.Cart;

public class RemoveItemFromCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IEnumerable<ICartObserver> _observers;

    public RemoveItemFromCartUseCase(ICartRepository cartRepository, IEnumerable<ICartObserver> observers)
    {
        _cartRepository = cartRepository;
        _observers = observers;
    }

    public async Task ExecuteAsync(Guid userId, Guid productId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        
        // Register observers
        foreach (var observer in _observers)
        {
            cart.RegisterObserver(observer);
        }

        cart.RemoveItem(productId);
        
        await _cartRepository.SaveAsync(cart);
    }
}
