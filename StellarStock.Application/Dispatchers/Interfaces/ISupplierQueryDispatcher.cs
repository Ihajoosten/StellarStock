namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface ISupplierQueryDispatcher<TQuery, TResult> : IQueryDispatcher<TQuery, TResult>
        where TQuery : ISupplierQuery<TResult>
    {
        // Additional methods if needed
    }
}
