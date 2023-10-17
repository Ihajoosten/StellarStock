namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface ISupplierCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ISupplierCommand
    {
        Task<Dictionary<string, bool>> HandleCreateAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleUpdateAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleDeleteAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleActivateAsync(TCommand command);
        Task<Dictionary<string, bool>> HandleDeactivateAsync(TCommand command);
    }
}
