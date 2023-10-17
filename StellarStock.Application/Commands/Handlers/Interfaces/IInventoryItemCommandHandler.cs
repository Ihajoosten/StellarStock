namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface IInventoryItemCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : IInventoryItemCommand
    {
        Task<Dictionary<string, bool>> HandleCreateAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleUpdateAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleDeleteAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleIncreaseQuantityAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleDecreaseQuantityAsync(TCommand command);
    }
}
