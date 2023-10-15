using StellarStock.Application.Commands.WarehouseCommands;
using StellarStock.Domain.Aggregates;

namespace StellarStock.Application.Handlers.Base
{
    public class GenericCommandHandler<TCommand, TEntity> : IGenericCommandHandler<TCommand, TEntity> where TCommand : ICommand where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly ILogger<GenericCommandHandler<TCommand, TEntity>> _logger;
        private readonly Dictionary<Type, Func<TCommand, Task>> _commandHandlers;

        public GenericCommandHandler(IGenericRepository<TEntity> repository, ILogger<GenericCommandHandler<TCommand, TEntity>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _commandHandlers = new Dictionary<Type, Func<TCommand, Task>>
            {
                { typeof(InventoryItem), HandleInventoryItemCommand },
                { typeof(Supplier), HandleSupplierCommand },
                { typeof(Warehouse), HandleWarehouseCommand },
            };
        }

        public async Task HandleAsync(TCommand command, TEntity entity)
        {
            if (_commandHandlers.TryGetValue(entity.GetType(), out var handler))
            {
                await handler.Invoke(command);
            }
            else
            {
                _logger.LogError($"Unsupported entity type: {typeof(TEntity)}");
                throw new ArgumentException($"Unsupported entity type: {typeof(TEntity)}");
            }
        }

        private async Task HandleInventoryItemCommand(TCommand command)
        {
            switch (command)
            {
                case CreateInventoryItemCommand createInventoryItemCommand:
                    await HandleCreateInventoryItemAsync(createInventoryItemCommand);
                    break;
                case UpdateInventoryItemCommand updateInventoryItemCommand:
                    await HandleUpdateInventoryItemAsync(updateInventoryItemCommand);
                    break;
                case DeleteInventoryItemCommand deleteInventoryItemCommand:
                    await HandleDeleteInventoryItemAsync(deleteInventoryItemCommand);
                    break;
                default:
                    LogAndThrowUnsupportedCommand();
                    break;
            }
        }

        private async Task HandleSupplierCommand(TCommand command)
        {
            switch (command)
            {
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

        private async Task HandleWarehouseCommand(TCommand command)
        {
            switch (command)
            {
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

        // Inventory Item handlers
        private async Task HandleCreateInventoryItemAsync(CreateInventoryItemCommand command)
        {
            try
            {
                var inventoryAggregate = new InventoryAggregate(null);
                inventoryAggregate.CreateInventoryItem(
                    command.Name,
                    command.Description,
                    command.Category,
                    command.PopularityScore,
                    command.ProductCode,
                    command.Quantity,
                    command.Money,
                    command.WarehouseId,
                    command.SupplierId,
                    command.ValidityPeriod
                );

                await _repository.AddAsync(inventoryAggregate.InventoryItem as TEntity);

                // Log successful creation
                _logger.LogInformation($"Inventory item created: {inventoryAggregate.InventoryItem.Id}");
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error in HandleCreateInventoryItemAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in CreateInventoryItemCommand");
            }
        }

        private async Task HandleUpdateInventoryItemAsync(UpdateInventoryItemCommand command)
        {
            try
            {
                if (await _repository.GetByIdAsync(command.InventoryItemId) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);
                    itemAggregate.UpdateItem(
                        command.NewName,
                        command.NewDescription,
                        command.NewCategory,
                        command.NewProductCode,
                        command.NewPopularityScore,
                        command.NewQuantity,
                        command.NewMoney
                        );

                    await _repository.UpdateAsync(itemAggregate.InventoryItem as TEntity);

                    // Log successful update
                    _logger.LogInformation($"Inventory item updated: {item.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error in HandleUpdateInventoryItemAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in UpdateInventoryItemCommand");
            }
        }

        private async Task HandleDeleteInventoryItemAsync(DeleteInventoryItemCommand command)
        {
            try
            {
                //var item = await _repository.GetByIdAsync(command.Id) as InventoryItem;
                if (await _repository.GetByIdAsync(command.Id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);

                    itemAggregate.RemoveItem();

                    await _repository.RemoveAsync(item.Id);

                    // Log successful deletion
                    _logger.LogInformation($"Inventory item deleted: {item.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error in HandleDeleteInventoryItemAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in DeleteInventoryItemCommand");
            }
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
                _logger.LogError(ex, "Error in HandleCreateSupplierAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in CreateSupplierCommand");
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
                _logger.LogError(ex, "Error in HandleUpdateSupplierAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in UpdateSupplierCommand");
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
                _logger.LogError(ex, "Error in HandleDeleteSupplierAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in DeleteSupplierCommand");
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
                _logger.LogError(ex, "Error in HandleActivateSupplierAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in ActivateSupplierCommand");
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
                _logger.LogError(ex, "Error in HandleDeactivateSupplierAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in DeactivateSupplierCommand");
            }
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
                _logger.LogError(ex, "Error in HandleCreateWarehouseAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in CreateWarehouseCommand");
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
                _logger.LogError(ex, "Error in HandleUpdateWarehouseAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in UpdateWarehouseCommand");
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
                _logger.LogError(ex, "Error in HandleDeleteWarehouseAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in DeleteWarehouseCommand");
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
                _logger.LogError(ex, "Error in HandleCloseWarehouseAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in CloseWarehouseCommand");
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
                _logger.LogError(ex, "Error in HandleReopenWarehouseAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in ReopenWarehouseCommand");
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
                _logger.LogError(ex, "Error in HandleMoveWarehouseAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in MoveWarehouseCommand");
            }
        }
    }
}