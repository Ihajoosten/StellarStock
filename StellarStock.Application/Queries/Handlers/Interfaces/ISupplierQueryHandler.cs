namespace StellarStock.Application.Queries.Handlers.Interfaces
{
    public interface ISupplierQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
     where TQuery : ISupplierQuery<TResult>
    {
        Task<TResult> HandleGetActiveAsync(TQuery query);
        Task<TResult> HandleGetByIdAsync(TQuery query);
        Task<TResult> HandleGetByCityAsync(TQuery query);
        Task<TResult> HandleGetByRegionAsync(TQuery query);
        Task<TResult> HandleGetExpiringSoonAsync(TQuery query);
    }
}
