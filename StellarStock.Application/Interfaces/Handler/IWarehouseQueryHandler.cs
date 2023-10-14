namespace StellarStock.Application.Interfaces.Handler
{
    public interface IWarehouseQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
