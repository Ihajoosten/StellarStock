namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface ISupplierCommandDispatcher<TCommand> : ICommandDispatcher<TCommand>
        where TCommand : ISupplierCommand
    {
        // Additional methods if needed
    }
}
