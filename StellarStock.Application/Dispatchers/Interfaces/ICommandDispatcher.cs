namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface ICommandDispatcher
    {
        Task<bool> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
