namespace StellarStock.Application.Commands.Handlers
{
    public class SupplierCommandHandler<TCommand> : ISupplierCommandHandler<TCommand>
            where TCommand : ISupplierCommand
    {
        private readonly IGenericRepository<Supplier> _repository;
        private readonly ILogger<SupplierCommandHandler<TCommand>> _logger;

        public SupplierCommandHandler(IGenericRepository<Supplier> repository, ILogger<SupplierCommandHandler<TCommand>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Dictionary<string, bool>> HandleAsync(TCommand command)
        {
            return command switch
            {
                CreateSupplierCommand => await HandleCreateAsync(command),
                UpdateSupplierCommand => await HandleUpdateAsync(command),
                DeleteSupplierCommand => await HandleDeleteAsync(command),
                ActivateSupplierCommand => await HandleActivateAsync(command),
                DeactivateSupplierCommand => await HandleDeactivateAsync(command),
                _ => await LogAndThrowUnsupportedCommand()
            };
        }

        private Task<Dictionary<string, bool>> LogAndThrowUnsupportedCommand()
        {
            _logger.LogError($"Unsupported command type: {typeof(TCommand)}");
            throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
        }

        public async Task<Dictionary<string, bool>> HandleCreateAsync(TCommand command)
        {
            try
            {
                if (command is not CreateSupplierCommand createCommand)
                {
                    _logger.LogError("Invalid command type for HandleCreateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                var supplier = new Supplier
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createCommand.Name,
                    Phone = createCommand.Phone,
                    ContactEmail = createCommand.ContactEmail,
                    Address = createCommand.Address,
                    ValidityPeriod = createCommand.ValidityPeriod,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var added = await _repository.AddAsync(supplier);

                if (added)
                {
                    _logger.LogInformation($"Supplier created: {supplier.Id}");
                    return new Dictionary<string, bool> { { supplier.Id, true } };
                }
                else
                {
                    _logger.LogInformation($"Supplier creation failed: {supplier.Id}");
                    return new Dictionary<string, bool> { { supplier.Id, false } };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleCreateAsync Supplier :: {ex.Message}");
                throw new Exception($"Error in HandleCreateAsync Supplier :: {ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleUpdateAsync(TCommand command)
        {
            try
            {
                if (command is not UpdateSupplierCommand updateCommand)
                {
                    _logger.LogError("Invalid command type for HandleUpdateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(updateCommand.Id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate?.UpdateSupplier(updateCommand.NewName, updateCommand.NewPhone, updateCommand.NewContactEmail, updateCommand.NewAddress);

                    var updated = await _repository.UpdateAsync(supplierAggregate.Supplier!);

                    if (updated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Supplier updated: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, true } };
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Supplier updated failed: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, false } };
                    }
                }
                else
                {
                    // Log failed update
                    _logger.LogInformation($"Supplier updated failed: {updateCommand.Id}");
                    return new Dictionary<string, bool> { { updateCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleUpdateAsync Supplier :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleDeleteAsync(TCommand command)
        {
            try
            {
                if (command is not DeleteSupplierCommand deleteCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleDeleteAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(deleteCommand.Id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.DeleteSupplier();

                    var deleted = await _repository.RemoveAsync(supplier.Id!);
                    if (deleted)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Supplier deleted: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, true } };
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Supplier deleting failed: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, false } };
                    }
                }
                else
                {
                    // Log failed deletion
                    _logger.LogInformation($"Supplier deleting failed: {deleteCommand.Id}");
                    return new Dictionary<string, bool> { { deleteCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleDeleteAsync Supplier :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleActivateAsync(TCommand command)
        {
            try
            {
                if (command is not ActivateSupplierCommand activateCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleActivateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(activateCommand.Id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.ActivateSupplier();

                    var activated = await _repository.UpdateAsync(supplierAggregate.Supplier!);
                    if (activated)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Supplier activated: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, true } };
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Supplier activating failed: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, false } };
                    }
                }
                else
                {
                    // Log failed deletion
                    _logger.LogInformation($"Supplier activating failed: {activateCommand.Id}");
                    return new Dictionary<string, bool> { { activateCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleActivateAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleActivateAsync Supplier :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleDeactivateAsync(TCommand command)
        {
            try
            {
                if (command is not DeactivateSupplierCommand deactivateCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleDeactivateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(deactivateCommand.Id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.DeactivateSupplier();

                    var deactivated = await _repository.UpdateAsync(supplierAggregate.Supplier!);
                    if (deactivated)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Supplier deactivated: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, true } };
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Supplier deactivating failed: {supplier.Id}");
                        return new Dictionary<string, bool> { { supplier.Id!, false } };
                    }
                }
                else
                {
                    // Log failed deletion
                    _logger.LogInformation($"Supplier deactivating failed: {deactivateCommand.Id}");
                    return new Dictionary<string, bool> { { deactivateCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeactivateAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleDeactivateAsync Supplier :: ${ex.Message}");
            }
        }
    }
}