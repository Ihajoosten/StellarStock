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

        public async Task HandleAsync(TCommand command)
        {
            switch (command)
            { // Warehouse Commands
                case CreateWarehouseCommand createWarehouseCommand:
                    await HandleCreateWarehouseAsync(createWarehouseCommand);
                    break;
                case UpdateWarehouseCommand updateWarehouseCommand:
                    await HandleUpdateWarehouseAsync(updateWarehouseCommand);
                    break;
                case DeleteWarehouseCommand deleteWarehouseCommand:
                    await HandleDeleteWarehouseAsync(deleteWarehouseCommand);
                    break;
                case CloseWarehouseCommand closeWarehouseCommand:
                    await HandleCloseWarehouseAsync(closeWarehouseCommand);
                    break;
                case ReopenWarehouseCommand reopenWarehouseCommand:
                    await HandleReopenWarehouseAsync(reopenWarehouseCommand);
                    break;
                case MoveWarehouseCommand moveWarehouseCommand:
                    await HandleMoveWarehouseAsync(moveWarehouseCommand);
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

        // Warehouse handlers
        private async Task HandleCreateWarehouseAsync(CreateWarehouseCommand command)
        {
            try
            {
                var warehouseAggregate = new WarehouseAggregate(null);
                warehouseAggregate.CreateWarehouse(command.Name, command.Phone, command.Address, true);

                await _repository.AddAsync(warehouseAggregate.Warehouse as TEntity);

                // Log successful creation
                _logger.LogInformation($"Warehouse created: {warehouseAggregate.Warehouse.Id}");
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CreateWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task HandleUpdateWarehouseAsync(UpdateWarehouseCommand updateWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(updateWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.UpdateWarehouse(updateWarehouseCommand.NewName, updateWarehouseCommand.NewPhone, updateWarehouseCommand.NewAddress);

                    await _repository.UpdateAsync(warehouseAggregate.Warehouse as TEntity);

                    // Log successful creation
                    _logger.LogInformation($"Warehouse updated: {warehouseAggregate.Warehouse.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in UpdateWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task HandleDeleteWarehouseAsync(DeleteWarehouseCommand deleteWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(deleteWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.DeleteWarehouse();

                    await _repository.RemoveAsync(warehouseAggregate.Warehouse.Id);

                    // Log successful creation
                    _logger.LogInformation($"Warehouse deleted: {warehouseAggregate.Warehouse.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in DeleteWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task HandleCloseWarehouseAsync(CloseWarehouseCommand closeWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(closeWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.CloseWarehouse(warehouse.IsOpen);

                    await _repository.UpdateAsync(warehouseAggregate.Warehouse as TEntity);

                    // Log successful creation
                    _logger.LogInformation($"Warehouse closed: {warehouseAggregate.Warehouse.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCloseWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CloseWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task HandleReopenWarehouseAsync(ReopenWarehouseCommand reopenWarehouseCommand)
        {
            try
            {
                if (await _repository.GetByIdAsync(reopenWarehouseCommand.WarehouseId) is Warehouse warehouse)
                {
                    var warehouseAggregate = new WarehouseAggregate(warehouse);
                    warehouseAggregate.ReopenWarehouse(warehouse.IsOpen);

                    await _repository.UpdateAsync(warehouseAggregate.Warehouse as TEntity);

                    // Log successful creation
                    _logger.LogInformation($"Warehouse reopened: {warehouseAggregate.Warehouse.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleReopenWarehouseAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in ReopenWarehouseCommand :: ${ex.Message}");
            }
        }

        private async Task HandleMoveWarehouseAsync(MoveWarehouseCommand moveWarehouseCommand)
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

                    // Log successful creation
                    _logger.LogInformation($"Warehouse moved: {warehouseAggregate.Warehouse.Id}");
                }
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