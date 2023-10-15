namespace StellarStock.Application.Dispatchers
{
    public class QueryDispatcher<TQuery, TResult> : IQueryDispatcher<TQuery, TResult>
    {
        public Task<TResult> DispatchAsync(TQuery query)
        {
            // Implementation for dispatching queries
            // You can return the result based on the query
            return Task.FromResult(default(TResult));
        }
    }
}
