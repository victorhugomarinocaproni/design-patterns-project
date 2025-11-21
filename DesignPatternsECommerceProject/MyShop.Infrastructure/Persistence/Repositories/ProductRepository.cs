using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;
using MyShop.Infrastructure.Persistence.Sqlite;

namespace MyShop.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MyShopDbContext _context;

    public ProductRepository(MyShopDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
}
