using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;

namespace MyShop.Application.Commands;

public class CreateOrderCommand : ICommand
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name => "CreateOrder";

    private readonly Guid _userId;
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    
    // State for Undo
    private Guid? _createdOrderId;

    public CreateOrderCommand(Guid userId, ICartRepository cartRepository, IOrderRepository orderRepository)
    {
        _userId = userId;
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync()
    {
        var cart = await _cartRepository.GetByUserIdAsync(_userId);
        if (!cart.Items.Any()) throw new Exception("Cart is empty");

        var orderItems = cart.Items.Select(i => new OrderItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity)).ToList();
        var order = new Order(_userId, cart.TotalAmount(), orderItems);
        
        await _orderRepository.AddAsync(order);
        _createdOrderId = order.Id;

        // Clear cart
        await _cartRepository.ClearAsync(_userId);
    }

    public async Task UndoAsync()
    {
        if (_createdOrderId.HasValue)
        {
            var order = await _orderRepository.GetByIdAsync(_createdOrderId.Value);
            if (order != null)
            {
                order.Cancel();
                await _orderRepository.UpdateAsync(order);
            }
        }
    }
}
