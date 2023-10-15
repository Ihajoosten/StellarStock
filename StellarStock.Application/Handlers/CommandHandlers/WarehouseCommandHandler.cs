namespace StellarStock.Application.Handlers.CommandHandlers
{
    public class WarehouseCommandHandler<TCommand, TEntity> : IGenericCommandHandler<TCommand, TEntity> where TCommand : ICommand where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly ILogger _logger;

        public WarehouseCommandHandler(IGenericRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> HandleAsync(TCommand command)
        {
            return command switch
            { 
                // Warehouse Commands
                CreateWarehouseCommand createWarehouseCommand => await HandleCreateWarehouseAsync(createWarehouseCommand),
                UpdateWarehouseCommand updateWarehouseCommand => await HandleUpdateWarehouseAsync(updateWarehouseCommand),
                DeleteWarehouseCommand deleteWarehouseCommand => await HandleDeleteWarehouseAsync(deleteWarehouseCommand),
                CloseWarehouseCommand closeWarehouseCommand => await HandleCloseWarehouseAsync(closeWarehouseCommand),
                ReopenWarehouseCommand reopenWarehouseCommand => await HandleReopenWarehouseAsync(reopenWarehouseCommand),
                MoveWarehouseCommand moveWarehouseCommand => await HandleMoveWarehouseAsync(moveWarehouseCommand),
                _ => LogAndThrowUnsupportedCommand(),
            };
        }

        private string LogAndThrowUnsupportedCommand()
        {
            _logger.LogError($"Unsupported command type: {typeof(TCommand)}");
            throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
        }

        // Warehouse handlers
        private async Task<string> HandleCreateWarehouseAsync(CreateWarehouseCommand command)
        {
            try
            {
                var warehouseAggregate = new WarehouseAggregate(null);
                warehouseAggregate.CreateWarehouse(command.Name, command.Phone, command.Address, true);

                await _repository.AddAsync(warehouseAggregate.Warehouse as TEntity);

                // Log successful creation
                _logger.LogInformation($"Warehouse created: {warehouseAggregate.Warehouse.Id}");
                return warehouseAggregate.Warehouse.Id!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CreateWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleUpdateWarehouseAsync(UpdateWarehouseCommand updateWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(updateWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.UpdateWarehouse(updateWarehouseCommand.NewName, updateWarehouseCommand.NewPhone, updateWarehouseCommand.NewAddress);

                    await _repository.UpdateAsync(warehouseAggregate.Warehouse as TEntity);

                    // Log successful update
                    _logger.LogInformation($"Warehouse updated: {warehouseAggregate.Warehouse.Id}");
                    return warehouseAggregate.Warehouse.Id!;
                }

                // Log failure update
                _logger.LogInformation($"Warehouse updated failed: {updateWarehouseCommand.WarehouseId}");
                return updateWarehouseCommand.WarehouseId!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in UpdateWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleDeleteWarehouseAsync(DeleteWarehouseCommand deleteWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(deleteWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.DeleteWarehouse();

                    await _repository.RemoveAsync(warehouseAggregate.Warehouse.Id);

                    // Log successful deletion
                    _logger.LogInformation($"Warehouse removed: {warehouseAggregate.Warehouse.Id}");
                    return warehouseAggregate.Warehouse.Id!;
                }

                // Log failure deletion
                _logger.LogInformation($"Warehouse removal failed: {deleteWarehouseCommand.WarehouseId}");
                return deleteWarehouseCommand.WarehouseId!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in DeleteWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleCloseWarehouseAsync(CloseWarehouseCommand closeWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(closeWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.CloseWarehouse(warehouse.IsOpen);

                    await _repository.UpdateAsync(warehouseAggregate.Warehouse as TEntity);

                    // Log successful update
                    _logger.LogInformation($"Warehouse closed: {warehouseAggregate.Warehouse.Id}");
                    return warehouseAggregate.Warehouse.Id!;
                }

                // Log failure update
                _logger.LogInformation($"Warehouse update failed: {closeWarehouseCommand.WarehouseId}");
                return closeWarehouseCommand.WarehouseId!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCloseWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CloseWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleReopenWarehouseAsync(ReopenWarehouseCommand reopenWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(reopenWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.ReopenWarehouse(warehouse.IsOpen);

                    await _repository.UpdateAsync(warehouseAggregate.Warehouse as TEntity);

                    // Log successful update
                    _logger.LogInformation($"Warehouse reopened: {warehouseAggregate.Warehouse.Id}");
                    return warehouseAggregate.Warehouse.Id!;
                }

                // Log failure update
                _logger.LogInformation($"Warehouse update failed: {reopenWarehouseCommand.WarehouseId}");
                return reopenWarehouseCommand.WarehouseId!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleReopenWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in ReopenWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleMoveWarehouseAsync(MoveWarehouseCommand moveWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(moveWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.MoveWarehouse(
                        moveWarehouseCommand.NewAddress,
                        moveWarehouseCommand.NewCity,
                        moveWarehouseCommand.NewRegion,
                        moveWarehouseCommand.NewCountry,
                        moveWarehouseCommand.NewPostalCode
                        );

                    await _repository.UpdateAsync(warehouseAggregate.Warehouse as TEntity);

                    // Log successful update
                    _logger.LogInformation($"Warehouse moved: {warehouseAggregate.Warehouse.Id}");
                    return warehouseAggregate.Warehouse.Id!;
                }

                // Log failure update
                _logger.LogInformation($"Warehouse update failed: {moveWarehouseCommand.WarehouseId}");
                return moveWarehouseCommand.WarehouseId!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleMoveWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in MoveWarehouseCommand :: ${ex.Message}");
            }
        }
    }
}