namespace StellarStock.Application.Commands.Handlers.Interfaces
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<Dictionary<string, bool>> HandleAsync(TCommand command);
    }
}
