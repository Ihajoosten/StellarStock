namespace StellarStock.Application.Interfaces.Base
{
    public interface IGenericQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
