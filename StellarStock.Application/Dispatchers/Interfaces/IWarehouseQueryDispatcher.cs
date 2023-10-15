namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface IWarehouseQueryDispatcher<TQuery, TResult> : IQueryDispatcher<TQuery, TResult>
        where TQuery : IWarehouseQuery<TResult>
    {
        // Additional methods if needed
    }
}
