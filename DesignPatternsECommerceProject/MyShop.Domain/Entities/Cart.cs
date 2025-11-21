using MyShop.Domain.Interfaces;
using MyShop.Domain.ValueObjects;

namespace MyShop.Domain.Entities;

public class Cart
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; } // For simplicity, can be a session ID
    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    
    // Observer Pattern: List of observers (not persisted)
    private readonly List<ICartObserver> _observers = new();

    public Cart(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }

    // EF Core
    protected Cart() { }

    // Observer Management
    public void RegisterObserver(ICartObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void UnregisterObserver(ICartObserver observer)
    {
        _observers.Remove(observer);
    }

    // Business Logic
    public void AddItem(Product product, int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem != null)
        {
            existingItem.AddQuantity(quantity);
            NotifyItemAdded(existingItem); // Notify with updated item
        }
        else
        {
            var newItem = new CartItem(product, quantity);
            _items.Add(newItem);
            NotifyItemAdded(newItem);
        }
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            NotifyItemRemoved(item);
        }
    }

    public void Clear()
    {
        _items.Clear();
        NotifyCartCleared();
    }

    public Money TotalAmount()
    {
        if (!_items.Any()) return Money.Zero(Currency.BRL); // Default to BRL
        
        var firstCurrency = _items.First().UnitPrice.Currency;
        var total = Money.Zero(firstCurrency);
        
        foreach (var item in _items)
        {
            total += item.TotalPrice;
        }
        return total;
    }

    // Notification Methods
    private void NotifyItemAdded(CartItem item)
    {
        foreach (var observer in _observers)
        {
            observer.OnItemAdded(item);
        }
    }

    private void NotifyItemRemoved(CartItem item)
    {
        foreach (var observer in _observers)
        {
            observer.OnItemRemoved(item);
        }
    }

    private void NotifyCartCleared()
    {
        foreach (var observer in _observers)
        {
            observer.OnCartCleared();
        }
    }
}
