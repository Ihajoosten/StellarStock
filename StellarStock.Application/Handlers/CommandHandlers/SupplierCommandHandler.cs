namespace StellarStock.Application.Handlers.CommandHandlers
{
    public class SupplierCommandHandler<TCommand, TEntity> : IGenericCommandHandler<TCommand, TEntity> where TCommand : ICommand where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly ILogger _logger;

        public SupplierCommandHandler(IGenericRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(TCommand command)
        {
            switch (command)
            {
                // Supplier Commands
                case CreateSupplierCommand createSupplierCommand:
                    await HandleCreateSupplierAsync(createSupplierCommand);
                    break;
                case UpdateSupplierCommand updateSupplierCommand:
                    await HandleUpdateSupplierAsync(updateSupplierCommand);
                    break;
                case DeleteSupplierCommand deleteSupplierCommand:
                    await HandleDeleteSupplierAsync(deleteSupplierCommand);
                    break;
                case ActivateSupplierCommand activateSupplierCommand:
                    await HandleActivateSupplierAsync(activateSupplierCommand);
                    break;
                case DeactivateSupplierCommand deactivateSupplierCommand:
                    await HandleDeactivateSupplierAsync(deactivateSupplierCommand);
                    break;
                default:
                    LogAndThrowUnsupportedCommand();
                    break;
            }
        }

        private void LogAndThrowUnsupportedCommand()
        {
            _logger.LogError($"Unsupported command type: {typeof(TCommand)}");
            throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
        }

        // Supplier handlers
        private async Task HandleCreateSupplierAsync(CreateSupplierCommand command)
        {
            try
            {
                var supplierAggregate = new SupplierAggregate(null);
                supplierAggregate.CreateSupplier(command.Name, command.Phone, command.ContactEmail, command.Address, true, command.ValidityPeriod);

                await _repository.AddAsync(supplierAggregate.Supplier as TEntity);

                // Log successful creation
                _logger.LogInformation($"Supplier created: {supplierAggregate.Supplier.Id}");
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CreateSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task HandleUpdateSupplierAsync(UpdateSupplierCommand command)
        {
            try
            {
                if (await _repository.GetByIdAsync(command.SupplierId) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.UpdateSupplier(
                        command.NewName,
                        command.NewPhone,
                        command.NewContactEmail,
                        command.NewAddress);

                    await _repository.UpdateAsync(supplier as TEntity);

                    // Log successful update
                    _logger.LogInformation($"SUpplier updated: {supplier.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in UpdateSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task HandleDeleteSupplierAsync(DeleteSupplierCommand deleteSupplierCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(deleteSupplierCommand.Id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);

                    supplierAggregate.DeleteSupplier();

                    await _repository.RemoveAsync(supplier.Id);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in DeleteSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task HandleActivateSupplierAsync(ActivateSupplierCommand activateSupplierCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(activateSupplierCommand.SupplierId) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.ActivateSupplier();

                    await _repository.UpdateAsync(supplierAggregate.Supplier as TEntity);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleActivateSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in ActivateSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task HandleDeactivateSupplierAsync(DeactivateSupplierCommand deactivateSupplierCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(deactivateSupplierCommand.SupplierId) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.DeactivateSupplier();

                    await _repository.UpdateAsync(supplierAggregate.Supplier as TEntity);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeactivateSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in DeactivateSupplierCommand :: ${ex.Message}");
            }
        }
    }
}
