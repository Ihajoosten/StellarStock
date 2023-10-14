namespace StellarStock.Application.Interfaces.Handler
{
    public interface ISupplierQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
