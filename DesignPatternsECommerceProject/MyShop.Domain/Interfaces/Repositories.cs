using MyShop.Domain.Entities;

namespace MyShop.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task UpdateAsync(Order order);
}

public interface ICartRepository
{
    Task<Cart> GetByUserIdAsync(Guid userId);
    Task SaveAsync(Cart cart);
    Task ClearAsync(Guid userId);
}

public interface IPaymentGateway
{
    Task<string> ProcessPaymentAsync(decimal amount, string currency);
}

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
}

public interface ICommandLogRepository
{
    Task AddAsync(CommandLog log);
    Task<IEnumerable<CommandLog>> GetAllAsync();
}
