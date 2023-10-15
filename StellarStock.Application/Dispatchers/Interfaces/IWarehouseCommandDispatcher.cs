namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface IWarehouseCommandDispatcher<TCommand> : ICommandDispatcher<TCommand>
        where TCommand : IWarehouseCommand
    {
        // Additional methods if needed
    }
}
