namespace StellarStock.Application.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<bool> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = _serviceProvider.GetService(commandHandlerType)!;

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler not found for command type {command.GetType()}");
            }

            return await handler.HandleAsync((dynamic)command);
        }
    }
}
