namespace StellarStock.Application.Dispatchers
{
    public class CommandDispatcher<TCommand> : ICommandDispatcher<TCommand>
    {
        public Task DispatchAsync(TCommand command)
        {
            // Implementation for dispatching commands
            return Task.CompletedTask;
        }
    }
}
