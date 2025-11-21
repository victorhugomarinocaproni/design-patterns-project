using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.ValueObjects;

namespace MyShop.Infrastructure.Persistence.Sqlite;

public class MyShopDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<MyShop.Domain.Entities.Payment> Payments { get; set; }
    public DbSet<CommandLog> CommandLogs { get; set; }

    public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Order Configuration
        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(o => o.Id);
            b.OwnsOne(o => o.TotalAmount, m =>
            {
                m.Property(p => p.Amount).HasColumnName("TotalAmount");
                m.OwnsOne(p => p.Currency, c =>
                {
                    c.Property(x => x.Code).HasColumnName("CurrencyCode");
                    c.Property(x => x.Symbol).HasColumnName("CurrencySymbol");
                });
            });
            b.OwnsMany(o => o.Items, i =>
            {
                i.WithOwner().HasForeignKey("OrderId");
                i.HasKey(x => x.Id);
                i.OwnsOne(x => x.UnitPrice, m =>
                {
                    m.Property(p => p.Amount).HasColumnName("UnitPrice");
                    m.OwnsOne(p => p.Currency, c =>
                    {
                        c.Property(x => x.Code).HasColumnName("CurrencyCode");
                        c.Property(x => x.Symbol).HasColumnName("CurrencySymbol");
                    });
                });
            });
        });

        // Product Configuration
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);
            b.OwnsOne(p => p.Price, m =>
            {
                m.Property(p => p.Amount).HasColumnName("Price");
                m.OwnsOne(p => p.Currency, c =>
                {
                    c.Property(x => x.Code).HasColumnName("CurrencyCode");
                    c.Property(x => x.Symbol).HasColumnName("CurrencySymbol");
                });
            });
        });

        // Cart Configuration
        modelBuilder.Entity<Cart>(b =>
        {
            b.HasKey(c => c.Id);
            b.HasMany(c => c.Items).WithOne().HasForeignKey("CartId");
        });

        modelBuilder.Entity<CartItem>(b =>
        {
            b.HasKey(ci => ci.Id);
            b.OwnsOne(ci => ci.UnitPrice, m =>
            {
                m.Property(p => p.Amount).HasColumnName("UnitPrice");
                m.OwnsOne(p => p.Currency, c =>
                {
                    c.Property(x => x.Code).HasColumnName("CurrencyCode");
                    c.Property(x => x.Symbol).HasColumnName("CurrencySymbol");
                });
            });
        });
        
        // Payment Configuration
        modelBuilder.Entity<MyShop.Domain.Entities.Payment>(b =>
        {
            b.HasKey(p => p.Id);
            b.OwnsOne(p => p.Amount, m =>
            {
                m.Property(p => p.Amount).HasColumnName("Amount");
                m.OwnsOne(p => p.Currency, c =>
                {
                    c.Property(x => x.Code).HasColumnName("CurrencyCode");
                    c.Property(x => x.Symbol).HasColumnName("CurrencySymbol");
                });
            });
        });
    }
}
