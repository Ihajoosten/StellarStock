namespace StellarStock.Application.Interfaces.Handler.Base
{
    public interface IGenericCommandHandler<TCommand, TEntity> where TCommand : ICommand where TEntity : class
    {
        Task<string> HandleAsync(TCommand command);
    }
}
