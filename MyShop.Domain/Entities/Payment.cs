using MyShop.Domain.ValueObjects;

namespace MyShop.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Money Amount { get; private set; }
    public string Method { get; private set; }
    public string Status { get; private set; }
    public string? TransactionId { get; private set; }
    public DateTime ProcessedAt { get; private set; }

    public Payment(Guid orderId, Money amount, string method)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Method = method;
        Status = "Pending";
        ProcessedAt = DateTime.UtcNow;
    }

    protected Payment() { }

    public void Complete(string transactionId)
    {
        Status = "Completed";
        TransactionId = transactionId;
    }

    public void Fail()
    {
        Status = "Failed";
    }
}
