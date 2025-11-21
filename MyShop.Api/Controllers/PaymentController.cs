using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Commands;
using MyShop.Application.Services;
using MyShop.Domain.Interfaces;

namespace MyShop.Api.Controllers;

[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly CommandInvoker _commandInvoker;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentStrategyFactory _strategyFactory;

    public PaymentController(
        CommandInvoker commandInvoker,
        IOrderRepository orderRepository,
        IPaymentStrategyFactory strategyFactory)
    {
        _commandInvoker = commandInvoker;
        _orderRepository = orderRepository;
        _strategyFactory = strategyFactory;
    }

    [HttpPost("method")]
    public async Task<IActionResult> SetMethod([FromBody] SetMethodRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order == null) return NotFound("Order not found");

        // Validate strategy exists
        try
        {
            _strategyFactory.GetStrategy(request.Method);
        }
        catch
        {
            return BadRequest("Invalid payment method. Available: Credit, Debit, Pix, Boleto");
        }

        order.SetPaymentMethod(request.Method);
        await _orderRepository.UpdateAsync(order);
        return Ok(new { Message = $"Payment method set to {request.Method}" });
    }

    [HttpPost("process")]
    public async Task<IActionResult> Process([FromHeader(Name = "X-User-Id")] Guid userId, [FromBody] ProcessPaymentRequest request)
    {
        try
        {
            var command = new ProcessPaymentCommand(request.OrderId, _orderRepository, _strategyFactory);
            await _commandInvoker.ExecuteCommandAsync(command, userId);
            return Ok(new { Message = "Payment processed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("methods")]
    public IActionResult GetMethods()
    {
        return Ok(new[] { "Credit", "Debit", "Pix", "Boleto" });
    }
}

public record SetMethodRequest(Guid OrderId, string Method);
public record ProcessPaymentRequest(Guid OrderId);
