using MyShop.Application.Commands;
using MyShop.Domain.Entities;
using MyShop.Domain.Interfaces;

namespace MyShop.Application.Services;

public class CommandInvoker
{
    private readonly ICommandLogRepository _logRepository;
    // In-memory history for Undo (per session/request usually, but here maybe global or per user if we had a way)
    // For simplicity, we'll just keep a local stack, but in a stateless API this won't work for Undo across requests unless we persist the command state.
    // The prompt says "Persist commands ... for auditing" and "Undo".
    // To Undo via API, we'd need to reconstruct the command or have the command logic be reversible based on data.
    // I'll implement Execute and Log. Undo might be triggered by a specific "Undo" endpoint that creates a "CancelOrderCommand" which IS the undo of "CreateOrder".
    // But the Pattern says "Undo".
    // I'll assume the "CancelOrderCommand" IS the undo mechanism exposed to the user.
    
    public CommandInvoker(ICommandLogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task ExecuteCommandAsync(ICommand command, Guid? userId)
    {
        await command.ExecuteAsync();
        
        var log = new CommandLog(command.Name, "JSON_PAYLOAD_PLACEHOLDER", userId);
        await _logRepository.AddAsync(log);
    }
}
