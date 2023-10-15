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

        public async Task<bool> HandleAsync(TCommand command)
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

        private Task<bool> LogAndThrowUnsupportedCommand()
        {
            _logger.LogError($"Unsupported command type: {typeof(TCommand)}");
            throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
        }

        public async Task<bool> HandleCreateAsync(TCommand command)
        {
            try
            {
                var name = (command as CreateSupplierCommand)!.Name;
                var phone = (command as CreateSupplierCommand)!.Phone;
                var email = (command as CreateSupplierCommand)!.ContactEmail;
                var address = (command as CreateSupplierCommand)!.Address;
                var validityPeriod = (command as CreateSupplierCommand)!.ValidityPeriod;

                var supplierAggregate = new SupplierAggregate(null);
                supplierAggregate.CreateSupplier(name, phone, email, address, true, validityPeriod);

                var created = await _repository.AddAsync(supplierAggregate.Supplier);

                if (created)
                {
                    // Log successful creation
                    _logger.LogInformation($"Supplier created: {supplierAggregate.Supplier.Id}");
                    return created;
                }
                else
                {
                    // Log failed update
                    _logger.LogInformation($"Supplier creation failed:  {supplierAggregate.Supplier.Id}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleCreateAsync Supplier :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleUpdateAsync(TCommand command)
        {
            try
            {
                var id = (command as UpdateSupplierCommand)!.Id;
                var name = (command as UpdateSupplierCommand)!.NewName;
                var phone = (command as UpdateSupplierCommand)!.NewPhone;
                var email = (command as UpdateSupplierCommand)!.NewContactEmail;
                var address = (command as UpdateSupplierCommand)!.NewAddress;

                if (await _repository.GetByIdAsync(id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.UpdateSupplier(name, phone, email, address);

                    var updated = await _repository.UpdateAsync(supplierAggregate.Supplier);

                    if (updated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Supplier updated: {supplier.Id}");
                        return updated;
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Supplier updated failed: {supplier.Id}");
                        return false;
                    }
                }

                // Log failed update
                _logger.LogInformation($"Supplier updated failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleUpdateAsync Supplier :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleDeleteAsync(TCommand command)
        {
            try
            {
                var id = (command as DeleteSupplierCommand)!.Id;

                if (await _repository.GetByIdAsync(id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.DeleteSupplier();

                    var deleted = await _repository.RemoveAsync(supplier.Id);

                    if (deleted)
                    {
                        // Log successful update
                        _logger.LogInformation($"Supplier deleted: {supplier.Id}");
                        return deleted;
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Supplier deleted failed: {supplier.Id}");
                        return false;
                    }
                }

                // Log failed update
                _logger.LogInformation($"Supplier deleted failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleDeleteAsync Supplier :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleActivateAsync(TCommand command)
        {
            try
            {
                var id = (command as ActivateSupplierCommand)!.Id;

                if (await _repository.GetByIdAsync(id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.ActivateSupplier();

                    var activated = await _repository.UpdateAsync(supplierAggregate.Supplier);

                    if (activated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Supplier activated: {supplier.Id}");
                        return activated;
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Supplier activated failed: {supplier.Id}");
                        return false;
                    }
                }

                // Log failed update
                _logger.LogInformation($"Supplier activated failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleActivateAsync Supplier :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleActivateAsync Supplier :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleDeactivateAsync(TCommand command)
        {
            try
            {
                var id = (command as DeactivateSupplierCommand)!.Id;

                if (await _repository.GetByIdAsync(id) is Supplier supplier)
                {
                    var supplierAggregate = new SupplierAggregate(supplier);
                    supplierAggregate.DeactivateSupplier();

                    var deactivated = await _repository.UpdateAsync(supplierAggregate.Supplier);

                    if (deactivated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Supplier deactivated: {supplier.Id}");
                        return deactivated;
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Supplier deactivated failed: {supplier.Id}");
                        return false;
                    }
                }

                // Log failed update
                _logger.LogInformation($"Supplier deactivated failed: {id}");
                return false;
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