namespace StellarStock.Application.Dispatchers
{
    public class SupplierCommandDispatcher<TCommand> : ISupplierCommandDispatcher<TCommand>
        where TCommand : ISupplierCommand
    {
        public Task DispatchAsync(TCommand command)
        {
            // Implementation for dispatching supplier commands
            return Task.CompletedTask;
        }
    }
}
