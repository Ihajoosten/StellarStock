namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface IInventoryItemCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : IInventoryItemCommand
    {
        Task<bool> HandleCreateAsync(TCommand command);
        Task<bool> HandleUpdateAsync(TCommand command);
        Task<bool> HandleDeleteAsync(TCommand command);
    }
}
