namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<bool> HandleAsync(TCommand command);
    }
}
