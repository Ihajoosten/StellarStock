namespace StellarStock.Application.Commands.Handlers
{
    public class WarehouseCommandHandler<TCommand> : IWarehouseCommandHandler<TCommand>
        where TCommand : IWarehouseCommand
    {
        private readonly IGenericRepository<Warehouse> _repository;
        private readonly ILogger<WarehouseCommandHandler<TCommand>> _logger;

        public WarehouseCommandHandler(IGenericRepository<Warehouse> repository, ILogger<WarehouseCommandHandler<TCommand>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> HandleAsync(TCommand command)
        {
            return command switch
            {
                CreateWarehouseCommand => await HandleCreateAsync(command),
                UpdateWarehouseCommand => await HandleUpdateAsync(command),
                DeleteWarehouseCommand => await HandleDeleteAsync(command),
                ReopenWarehouseCommand => await HandleReopenAsync(command),
                CloseWarehouseCommand => await HandleCloseAsync(command),
                MoveWarehouseCommand => await HandleMoveAsync(command),
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
                var name = (command as CreateWarehouseCommand)!.Name;
                var phone = (command as CreateWarehouseCommand)!.Phone;
                var address = (command as CreateWarehouseCommand)!.Address;

                var warehouseAggregate = new WarehouseAggregate(null);
                warehouseAggregate.CreateWarehouse(name, phone, address, true);

                var created = await _repository.AddAsync(warehouseAggregate.Warehouse);

                if (created)
                {
                    // Log successful creation
                    _logger.LogInformation($"Warehouse created: {warehouseAggregate.Warehouse.Id}");
                    return true;
                }
                else
                {
                    // Log failed creation
                    _logger.LogInformation($"Warehouse creation failed:  {warehouseAggregate.Warehouse.Id}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleCreateAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleUpdateAsync(TCommand command)
        {
            try
            {
                var id = (command as UpdateWarehouseCommand)!.Id;
                var name = (command as UpdateWarehouseCommand)!.NewName;
                var phone = (command as UpdateWarehouseCommand)!.NewPhone;
                var address = (command as UpdateWarehouseCommand)!.NewAddress;

                if (await _repository.GetByIdAsync(id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.UpdateWarehouse(name, phone, address);

                    var updated = await _repository.UpdateAsync(warehouseAggregate.Warehouse);

                    if (updated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Warehouse updated: {warehouse.Id}");
                        return true;
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Warehouse update failed: {warehouse.Id}");
                        return false;
                    }
                }

                // Log failed update
                _logger.LogInformation($"Warehouse update failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleUpdateAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleDeleteAsync(TCommand command)
        {
            try
            {
                var id = (command as DeleteWarehouseCommand)!.Id;

                if (await _repository.GetByIdAsync(id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.DeleteWarehouse();

                    var deleted = await _repository.RemoveAsync(warehouse.Id);

                    if (deleted)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Warehouse deleted: {warehouse.Id}");
                        return true;
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Warehouse deletion failed: {warehouse.Id}");
                        return false;
                    }
                }

                // Log failed deletion
                _logger.LogInformation($"Warehouse deletion failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleDeleteAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleCloseAsync(TCommand command)
        {
            try
            {
                var id = (command as CloseWarehouseCommand)!.Id;

                if (await _repository.GetByIdAsync(id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.CloseWarehouse(warehouse.IsOpen);

                    var closed = await _repository.UpdateAsync(warehouseAggregate.Warehouse);

                    if (closed)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Warehouse closed: {warehouse.Id}");
                        return true;
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Warehouse closing failed: {warehouse.Id}");
                        return false;
                    }
                }

                // Log failed deletion
                _logger.LogInformation($"Warehouse closing failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCloseAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleCloseAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleReopenAsync(TCommand command)
        {
            try
            {
                var id = (command as ReopenWarehouseCommand)!.Id;

                if (await _repository.GetByIdAsync(id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.ReopenWarehouse(warehouse.IsOpen);

                    var reopened = await _repository.UpdateAsync(warehouseAggregate.Warehouse);

                    if (reopened)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Warehouse reopened: {warehouse.Id}");
                        return true;
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Warehouse reopening failed: {warehouse.Id}");
                        return false;
                    }
                }

                // Log failed deletion
                _logger.LogInformation($"Warehouse reopening failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleReopenAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleReopenAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleMoveAsync(TCommand command)
        {
            try
            {
                var id = (command as MoveWarehouseCommand)!.Id;
                var address = (command as MoveWarehouseCommand)!.NewAddress;
                var city = (command as MoveWarehouseCommand)!.NewCity;
                var postalCode = (command as MoveWarehouseCommand)!.NewPostalCode;
                var country = (command as MoveWarehouseCommand)!.NewCountry;
                var region = (command as MoveWarehouseCommand)!.NewRegion;

                if (await _repository.GetByIdAsync(id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.MoveWarehouse(address, city, region, country, postalCode);

                    var moved = await _repository.UpdateAsync(warehouseAggregate.Warehouse);

                    if (moved)
                    {
                        // Log successful update
                        _logger.LogInformation($"Warehouse moved: {warehouse.Id}");
                        return true;
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Warehouse relocation failed: {warehouse.Id}");
                        return false;
                    }
                }

                // Log failed update
                _logger.LogInformation($"Warehouse relocation failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleMoveAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleMoveAsync Warehouse :: ${ex.Message}");
            }
        }
    }
}
