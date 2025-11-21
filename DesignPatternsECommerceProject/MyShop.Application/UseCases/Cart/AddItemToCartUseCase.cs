using MyShop.Domain.Interfaces;

namespace MyShop.Application.UseCases.Cart;

public class AddItemToCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEnumerable<ICartObserver> _observers;

    public AddItemToCartUseCase(ICartRepository cartRepository, IProductRepository productRepository, IEnumerable<ICartObserver> observers)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _observers = observers;
    }

    public async Task ExecuteAsync(Guid userId, Guid productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) throw new Exception("Product not found");

        var cart = await _cartRepository.GetByUserIdAsync(userId);
        
        // Register observers (Observer Pattern)
        foreach (var observer in _observers)
        {
            cart.RegisterObserver(observer);
        }

        cart.AddItem(product, quantity);
        
        await _cartRepository.SaveAsync(cart);
    }
}
