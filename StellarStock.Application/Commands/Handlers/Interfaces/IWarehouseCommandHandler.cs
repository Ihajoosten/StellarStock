namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface IWarehouseCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : IWarehouseCommand
    {
        Task<bool> HandleCreateAsync(TCommand command);
        Task<bool> HandleUpdateAsync(TCommand command);
        Task<bool> HandleDeleteAsync(TCommand command);
        Task<bool> HandleCloseAsync(TCommand command);
        Task<bool> HandleReopenAsync(TCommand command);
        Task<bool> HandleMoveAsync(TCommand command);
    }
}
