namespace StellarStock.Application.Dispatchers
{
    public class InventoryItemCommandDispatcher<TCommand> : IInventoryItemCommandDispatcher<TCommand>
        where TCommand : IInventoryItemCommand
    {
        public Task DispatchAsync(TCommand command)
        {
            // Implementation for dispatching inventory item commands
            return Task.CompletedTask;
        }
    }
}
