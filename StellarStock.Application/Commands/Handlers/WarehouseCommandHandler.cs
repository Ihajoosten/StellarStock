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

        public async Task<Dictionary<string, bool>> HandleAsync(TCommand command)
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

        private Task<Dictionary<string, bool>> LogAndThrowUnsupportedCommand()
        {
            _logger.LogError($"Unsupported command type: {typeof(TCommand)}");
            throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
        }

        public async Task<Dictionary<string, bool>> HandleCreateAsync(TCommand command)
        {
            try
            {
                if (command is not CreateWarehouseCommand createCommand)
                {
                    _logger.LogError("Invalid command type for HandleCreateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                var warehouse = new Warehouse
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createCommand.Name,
                    Phone = createCommand.Phone,
                    Address = createCommand.Address,
                    IsOpen = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var added = await _repository.AddAsync(warehouse);

                if (added)
                {
                    _logger.LogInformation($"Warehouse created: {warehouse.Id}");
                    return new Dictionary<string, bool> { { warehouse.Id, true } };
                }
                else
                {
                    _logger.LogInformation($"Warehouse creation failed: {warehouse.Id}");
                    return new Dictionary<string, bool> { { warehouse.Id, false } };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleCreateAsync Warehouse :: {ex.Message}");
                throw new Exception($"Error in HandleCreateAsync Warehouse :: {ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleUpdateAsync(TCommand command)
        {
            try
            {
                if (command is not UpdateWarehouseCommand updateCommand)
                {
                    _logger.LogError("Invalid command type for HandleUpdateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(updateCommand.Id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate?.UpdateWarehouse(updateCommand.NewName, updateCommand.NewPhone, updateCommand.NewAddress);

                    var updated = await _repository.UpdateAsync(warehouseAggregate.Warehouse!);

                    if (updated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Warehouse updated: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, true } };
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Warehouse updated failed: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, false } };
                    }
                }
                else
                {
                    // Log failed update
                    _logger.LogInformation($"Warehouse updated failed: {updateCommand.Id}");
                    return new Dictionary<string, bool> { { updateCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleUpdateAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleDeleteAsync(TCommand command)
        {
            try
            {
                if (command is not DeleteWarehouseCommand deleteCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleDeleteAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(deleteCommand.Id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.DeleteWarehouse();

                    var deleted = await _repository.RemoveAsync(warehouse.Id!);
                    if (deleted)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Warehouse deleted: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, true } };
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Warehouse deleting failed: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, false } };
                    }
                }
                else
                {
                    // Log failed deletion
                    _logger.LogInformation($"Warehouse deleting failed: {deleteCommand.Id}");
                    return new Dictionary<string, bool> { { deleteCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleDeleteAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleCloseAsync(TCommand command)
        {
            try
            {
                if (command is not CloseWarehouseCommand closeCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleCloseAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(closeCommand.Id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.CloseWarehouse(warehouse.IsOpen);

                    var closed = await _repository.UpdateAsync(warehouseAggregate.Warehouse!);

                    if (closed)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Warehouse closed: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, true } };

                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Warehouse closing failed: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, false } };

                    }
                }
                else
                {
                    // Log failed deletion
                    _logger.LogInformation($"Warehouse closing failed: {closeCommand.Id}");
                    return new Dictionary<string, bool> { { closeCommand.Id!, false } };
                }

            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCloseAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleCloseAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleReopenAsync(TCommand command)
        {
            try
            {
                if (command is not ReopenWarehouseCommand reopenCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleReopenAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(reopenCommand.Id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.ReopenWarehouse(warehouse.IsOpen);

                    var reopened = await _repository.UpdateAsync(warehouseAggregate.Warehouse!);

                    if (reopened)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Warehouse reopened: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, true } };
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Warehouse reopening failed: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, false } };
                    }
                }
                else
                {
                    // Log failed deletion
                    _logger.LogInformation($"Warehouse reopening failed: {reopenCommand.Id}");
                    return new Dictionary<string, bool> { { reopenCommand.Id!, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleReopenAsync Warehouse :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleReopenAsync Warehouse :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleMoveAsync(TCommand command)
        {
            try
            {
                if (command is not MoveWarehouseCommand moveCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleMoveAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(moveCommand.Id) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.MoveWarehouse(moveCommand.NewAddress, moveCommand.NewCity, moveCommand.NewRegion, moveCommand.NewCountry, moveCommand.NewPostalCode);

                    var moved = await _repository.UpdateAsync(warehouseAggregate.Warehouse!);
                    if (moved)
                    {
                        // Log successful update
                        _logger.LogInformation($"Warehouse moved: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, true } };
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Warehouse relocation failed: {warehouse.Id}");
                        return new Dictionary<string, bool> { { warehouse.Id!, false } };
                    }
                }
                else
                {
                    // Log failed update
                    _logger.LogInformation($"Warehouse relocation failed: {moveCommand.Id}");
                    return new Dictionary<string, bool> { { moveCommand.Id!, false } };
                }
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
