using MyShop.Domain.ValueObjects;

namespace MyShop.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Money TotalAmount { get; private set; }
    public string Status { get; private set; } // Pending, Paid, Cancelled
    public string? PaymentMethod { get; private set; }
    
    // Simplified items for Order (could be a separate OrderItem entity, but for this scope string or JSON might suffice, or a list of OrderItems)
    // I'll use a list of OrderItem to be clean.
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Order(Guid userId, Money totalAmount, List<OrderItem> items)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        TotalAmount = totalAmount;
        Status = "Pending";
        _items = items;
    }

    // EF Core
    protected Order() { }

    public void SetPaymentMethod(string method)
    {
        PaymentMethod = method;
    }

    public void MarkAsPaid()
    {
        Status = "Paid";
    }

    public void Cancel()
    {
        Status = "Cancelled";
    }
}

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public OrderItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
    
    protected OrderItem() { }
}
