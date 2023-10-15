namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface ISupplierCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ISupplierCommand
    {
        Task<bool> HandleCreateAsync(TCommand command);
        Task<bool> HandleUpdateAsync(TCommand command);
        Task<bool> HandleDeleteAsync(TCommand command);
        Task<bool> HandleActivateAsync(TCommand command);
        Task<bool> HandleDeactivateAsync(TCommand command);
    }
}
