namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface ICommandDispatcher<TCommand>
    {
        Task DispatchAsync(TCommand command);
    }
}
