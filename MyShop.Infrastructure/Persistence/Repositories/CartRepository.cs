using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;
using MyShop.Infrastructure.Persistence.Sqlite;

namespace MyShop.Infrastructure.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly MyShopDbContext _context;

    public CartRepository(MyShopDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetByUserIdAsync(Guid userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart(userId);
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task SaveAsync(Cart cart)
    {
        // EF Core tracks changes, so just SaveChanges is usually enough if attached.
        // But to be safe/explicit:
        if (_context.Entry(cart).State == EntityState.Detached)
        {
            _context.Carts.Update(cart);
        }
        await _context.SaveChangesAsync();
    }

    public async Task ClearAsync(Guid userId)
    {
        var cart = await GetByUserIdAsync(userId);
        cart.Clear();
        await _context.SaveChangesAsync();
    }
}
