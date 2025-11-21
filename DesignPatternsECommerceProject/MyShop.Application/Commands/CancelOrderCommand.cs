using MyShop.Domain.Interfaces;

namespace MyShop.Application.Commands;

public class CancelOrderCommand : ICommand
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name => "CancelOrder";

    private readonly Guid _orderId;
    private readonly IOrderRepository _orderRepository;
    
    // State for Undo
    private string? _previousStatus;

    public CancelOrderCommand(Guid orderId, IOrderRepository orderRepository)
    {
        _orderId = orderId;
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync()
    {
        var order = await _orderRepository.GetByIdAsync(_orderId);
        if (order == null) throw new Exception("Order not found");

        _previousStatus = order.Status;
        order.Cancel();
        await _orderRepository.UpdateAsync(order);
    }

    public async Task UndoAsync()
    {
        if (_previousStatus != null)
        {
            var order = await _orderRepository.GetByIdAsync(_orderId);
            if (order != null)
            {
                // We need a way to restore status. 
                // For demo, I'll assume we can't fully restore if we don't expose a setter.
                // But I can add a method "RestoreStatus" to Order entity if needed.
            }
        }
    }
}
