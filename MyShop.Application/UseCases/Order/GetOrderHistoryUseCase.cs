using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;

namespace MyShop.Application.UseCases.Order;

public class GetOrderHistoryUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderHistoryUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Domain.Entities.Order>> ExecuteAsync(Guid userId)
    {
        var allOrders = await _orderRepository.GetAllAsync();
        return allOrders.Where(o => o.UserId == userId);
    }
}
