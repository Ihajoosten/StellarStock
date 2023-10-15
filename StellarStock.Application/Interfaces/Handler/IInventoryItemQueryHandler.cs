namespace StellarStock.Application.Interfaces.Handler
{
    public interface IInventoryItemQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task HandleAsync(TQuery query);
    }
}
