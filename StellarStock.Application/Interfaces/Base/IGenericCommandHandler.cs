namespace StellarStock.Application.Interfaces.Base
{
    public interface IGenericCommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
