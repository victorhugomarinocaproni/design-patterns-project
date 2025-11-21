using MyShop.Domain.ValueObjects;

namespace MyShop.Domain.Entities;

public class CartItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public Money TotalPrice => UnitPrice * Quantity;

    public CartItem(Product product, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = product.Id;
        ProductName = product.Name;
        UnitPrice = product.Price;
        Quantity = quantity;
    }

    // EF Core
    protected CartItem() { }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }
}
