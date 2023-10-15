namespace StellarStock.Application.Interfaces.Handler
{
    public interface IWarehouseQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task HandleAsync(TQuery query);
    }
}
