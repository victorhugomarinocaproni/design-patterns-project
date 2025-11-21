namespace MyShop.Domain.Entities;

public class CommandLog
{
    public Guid Id { get; private set; }
    public string CommandName { get; private set; }
    public string Payload { get; private set; } // JSON serialization of command data
    public DateTime ExecutedAt { get; private set; }
    public Guid? UserId { get; private set; }

    public CommandLog(string commandName, string payload, Guid? userId)
    {
        Id = Guid.NewGuid();
        CommandName = commandName;
        Payload = payload;
        ExecutedAt = DateTime.UtcNow;
        UserId = userId;
    }

    protected CommandLog() { }
}
