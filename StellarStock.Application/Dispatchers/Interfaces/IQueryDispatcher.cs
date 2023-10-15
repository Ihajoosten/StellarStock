namespace StellarStock.Application.Dispatchers.Interfaces
{
    public interface IQueryDispatcher<TQuery, TResult>
    {
        Task<TResult> DispatchAsync(TQuery query);
    }
}
