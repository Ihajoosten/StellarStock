namespace StellarStock.Application.Queries.Handlers.Interfaces
{
    public interface IInventoryItemQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IInventoryItemQuery<TResult>
    {
        Task<TResult> HandleGetInStockAsync(TQuery query);
        Task<TResult> HandleGetByIdAsync(TQuery query);
        Task<TResult> HandleGetByCategoryAsync(TQuery query);
        Task<TResult> HandleGetByWarehouseAsync(TQuery query);
        Task<TResult> HandleGetExpiringSoonAsync(TQuery query);
        Task<TResult> HandleGetLowStockAsync(TQuery query);
        Task<TResult> HandleGetItemsByPopularityScoreAsync(TQuery query);
        Task<TResult> HandleSearchAsync(TQuery query);
    }
}
