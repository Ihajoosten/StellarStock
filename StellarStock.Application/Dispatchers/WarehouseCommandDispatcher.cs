namespace StellarStock.Application.Dispatchers
{
    public class WarehouseCommandDispatcher<TCommand> : IWarehouseCommandDispatcher<TCommand>
        where TCommand : IWarehouseCommand
    {
        public Task DispatchAsync(TCommand command)
        {
            // Implementation for dispatching warehouse commands
            return Task.CompletedTask;
        }
    }
}
