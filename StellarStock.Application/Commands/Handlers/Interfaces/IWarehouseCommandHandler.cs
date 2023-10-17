namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface IWarehouseCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : IWarehouseCommand
    {
        Task<Dictionary<string, bool>> HandleCreateAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleUpdateAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleDeleteAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleCloseAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleReopenAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleMoveAsync(TCommand command);
    }
}
