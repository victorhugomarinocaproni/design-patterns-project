using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;
using MyShop.Infrastructure.Persistence.Sqlite;

namespace MyShop.Infrastructure.Persistence.Repositories;

public class CommandLogRepository : ICommandLogRepository
{
    private readonly MyShopDbContext _context;

    public CommandLogRepository(MyShopDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CommandLog log)
    {
        await _context.CommandLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommandLog>> GetAllAsync()
    {
        return await _context.CommandLogs.OrderByDescending(l => l.ExecutedAt).ToListAsync();
    }
}
