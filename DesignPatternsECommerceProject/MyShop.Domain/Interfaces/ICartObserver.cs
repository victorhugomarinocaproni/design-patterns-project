using MyShop.Domain.Entities;

namespace MyShop.Domain.Interfaces;

public interface ICartObserver
{
    void OnItemAdded(CartItem item);
    void OnItemRemoved(CartItem item);
    void OnCartCleared();
}
