namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface IInventoryItemCommandDispatcher<TCommand> : ICommandDispatcher<TCommand>
        where TCommand : IInventoryItemCommand
    {
        // Additional methods if needed
    }
}
