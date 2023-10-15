namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface IInventoryItemQueryDispatcher<TQuery, TResult> : IQueryDispatcher<TQuery, TResult>
        where TQuery : IInventoryItemQuery<TResult>
    {
        // Additional methods if needed
    }
}
