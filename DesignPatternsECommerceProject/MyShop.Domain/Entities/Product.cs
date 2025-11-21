using MyShop.Domain.ValueObjects;

namespace MyShop.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }
    public int StockQuantity { get; private set; }

    public Product(string name, Money price, int stockQuantity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    // EF Core constructor
    protected Product() { }

    public void DecreaseStock(int quantity)
    {
        if (StockQuantity < quantity)
            throw new Exceptions.DomainException("Insufficient stock");
        StockQuantity -= quantity;
    }
    
    public void IncreaseStock(int quantity)
    {
        StockQuantity += quantity;
    }
}
