namespace StellarStock.Application.Interfaces.Handler
{
    public interface ISupplierQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task HandleAsync(TQuery query);
    }
}
