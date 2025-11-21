using Microsoft.AspNetCore.Mvc;
using MyShop.Application.UseCases.Cart;
using MyShop.Domain.Interfaces;

namespace MyShop.Api.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly AddItemToCartUseCase _addItemUseCase;
    private readonly RemoveItemFromCartUseCase _removeItemUseCase;
    private readonly ICartRepository _cartRepository;

    public CartController(
        AddItemToCartUseCase addItemUseCase,
        RemoveItemFromCartUseCase removeItemUseCase,
        ICartRepository cartRepository)
    {
        _addItemUseCase = addItemUseCase;
        _removeItemUseCase = removeItemUseCase;
        _cartRepository = cartRepository;
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromHeader(Name = "X-User-Id")] Guid userId, [FromBody] AddItemRequest request)
    {
        try
        {
            await _addItemUseCase.ExecuteAsync(userId, request.ProductId, request.Quantity);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("items/{productId}")]
    public async Task<IActionResult> RemoveItem([FromHeader(Name = "X-User-Id")] Guid userId, Guid productId)
    {
        try
        {
            await _removeItemUseCase.ExecuteAsync(userId, productId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCart([FromHeader(Name = "X-User-Id")] Guid userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        return Ok(cart);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart([FromHeader(Name = "X-User-Id")] Guid userId)
    {
        await _cartRepository.ClearAsync(userId);
        return Ok();
    }
}

public record AddItemRequest(Guid ProductId, int Quantity);
