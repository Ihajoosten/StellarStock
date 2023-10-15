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

        public async Task<string> HandleAsync(TCommand command)
        {
            return command switch
            {
                // Supplier Commands
                CreateSupplierCommand createSupplierCommand => await HandleCreateSupplierAsync(createSupplierCommand),
                UpdateSupplierCommand updateSupplierCommand => await HandleUpdateSupplierAsync(updateSupplierCommand),
                DeleteSupplierCommand deleteSupplierCommand => await HandleDeleteSupplierAsync(deleteSupplierCommand),
                ActivateSupplierCommand activateSupplierCommand => await HandleActivateSupplierAsync(activateSupplierCommand),
                DeactivateSupplierCommand deactivateSupplierCommand => await HandleDeactivateSupplierAsync(deactivateSupplierCommand),
                _ => LogAndThrowUnsupportedCommand(),
            };
        }

        private string LogAndThrowUnsupportedCommand()
        {
            _logger.LogError($"Unsupported command type: {typeof(TCommand)}");
            throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
        }

        // Supplier handlers
        private async Task<string> HandleCreateSupplierAsync(CreateSupplierCommand command)
        {
            try
            {
                var supplierAggregate = new SupplierAggregate(null);
                supplierAggregate.CreateSupplier(command.Name, command.Phone, command.ContactEmail, command.Address, true, command.ValidityPeriod);

                await _repository.AddAsync(supplierAggregate.Supplier as TEntity);

                // Log successful creation
                _logger.LogInformation($"Supplier created: {supplierAggregate.Supplier.Id}");
                return supplierAggregate.Supplier.Id!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CreateSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleUpdateSupplierAsync(UpdateSupplierCommand command)
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
                    _logger.LogInformation($"Supplier updated: {supplier.Id}");
                    return supplier.Id!;
                }

                // Log failed update
                _logger.LogInformation($"Supplier updated failed - could not find supplier: {command.SupplierId}");
                return command.SupplierId;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in UpdateSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleDeleteSupplierAsync(DeleteSupplierCommand deleteSupplierCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(deleteSupplierCommand.Id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);

                    supplierAggregate.DeleteSupplier();

                    await _repository.RemoveAsync(supplier.Id);

                    // Log successful update
                    _logger.LogInformation($"Supplier removed: {supplier.Id}");
                    return supplier.Id!;
                }

                // Log failed update
                _logger.LogInformation($"Supplier removal failed: {deleteSupplierCommand.Id}");
                return deleteSupplierCommand.Id;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in DeleteSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleActivateSupplierAsync(ActivateSupplierCommand activateSupplierCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(activateSupplierCommand.SupplierId) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.ActivateSupplier();

                    await _repository.UpdateAsync(supplierAggregate.Supplier as TEntity);

                    // Log successful update
                    _logger.LogInformation($"Supplier activated: {supplier.Id}");
                    return supplier.Id!;
                }

                // Log failed update
                _logger.LogInformation($"Supplier activation failed: {activateSupplierCommand.SupplierId}");
                return activateSupplierCommand.SupplierId;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleActivateSupplierAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in ActivateSupplierCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleDeactivateSupplierAsync(DeactivateSupplierCommand deactivateSupplierCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(deactivateSupplierCommand.SupplierId) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.DeactivateSupplier();

                    await _repository.UpdateAsync(supplierAggregate.Supplier as TEntity);

                    // Log successful update
                    _logger.LogInformation($"Supplier deactivated: {supplier.Id}");
                    return supplier.Id!;
                }
                // Log failed update
                _logger.LogInformation($"Supplier deactivation failed: {deactivateSupplierCommand.SupplierId}");
                return deactivateSupplierCommand.SupplierId;
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
