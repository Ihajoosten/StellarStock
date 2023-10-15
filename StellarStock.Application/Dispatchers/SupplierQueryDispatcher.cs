namespace StellarStock.Application.Dispatchers
{
    public class SupplierQueryDispatcher<TQuery, TResult> : ISupplierQueryDispatcher<TQuery, TResult>
        where TQuery : ISupplierQuery<TResult>
    {
        public Task<TResult> DispatchAsync(TQuery query)
        {
            // Implementation for dispatching supplier queries
            // You can return the result based on the query
            return Task.FromResult(default(TResult));
        }
    }
}
