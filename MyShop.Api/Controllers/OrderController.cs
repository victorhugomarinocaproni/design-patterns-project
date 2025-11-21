using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Commands;
using MyShop.Application.Services;
using MyShop.Application.UseCases.Order;
using MyShop.Domain.Interfaces;

namespace MyShop.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly CommandInvoker _commandInvoker;
    private readonly GetOrderHistoryUseCase _historyUseCase;
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderController(
        CommandInvoker commandInvoker,
        GetOrderHistoryUseCase historyUseCase,
        ICartRepository cartRepository,
        IOrderRepository orderRepository)
    {
        _commandInvoker = commandInvoker;
        _historyUseCase = historyUseCase;
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromHeader(Name = "X-User-Id")] Guid userId)
    {
        try
        {
            var command = new CreateOrderCommand(userId, _cartRepository, _orderRepository);
            await _commandInvoker.ExecuteCommandAsync(command, userId);
            return Ok(new { Message = "Order created successfully", OrderId = command.Id }); // Ideally return the created Order ID from the command state
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetHistory([FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var orders = await _historyUseCase.ExecuteAsync(userId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder([FromHeader(Name = "X-User-Id")] Guid userId, Guid id)
    {
        try
        {
            var command = new CancelOrderCommand(id, _orderRepository);
            await _commandInvoker.ExecuteCommandAsync(command, userId);
            return Ok(new { Message = "Order cancelled" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
