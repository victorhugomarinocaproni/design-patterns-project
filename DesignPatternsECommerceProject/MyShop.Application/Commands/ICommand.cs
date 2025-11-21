namespace MyShop.Application.Commands;

public interface ICommand
{
    Guid Id { get; }
    string Name { get; }
    Task ExecuteAsync();
    Task UndoAsync();
}
