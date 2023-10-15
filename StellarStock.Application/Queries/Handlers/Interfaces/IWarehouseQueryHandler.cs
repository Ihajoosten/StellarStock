namespace StellarStock.Application.Queries.Handlers.Interfaces
{
    public interface IWarehouseQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IWarehouseQuery<TResult>
    {
        Task<TResult> HandleGetClosedAsync(TQuery query);
        Task<TResult> HandleGetOpenedAsync(TQuery query);
        Task<TResult> HandleGetByIdAsync(TQuery query);
        Task<TResult> HandleGetStockedItemsAsync(TQuery query);
        Task<TResult> HandleGetByCityAsync(TQuery query);
        Task<TResult> HandleGetByRegionAsync(TQuery query);
    }
}
