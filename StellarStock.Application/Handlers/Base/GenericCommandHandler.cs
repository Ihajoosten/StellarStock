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
        private async Task HandleCreateInventoryItemAsync(CreateInventoryItemCommand createCommand)
        {
            try
            {
                var item = new InventoryItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createCommand.Name,
                    Description = createCommand.Description,
                    Category = createCommand.Category,
                    PopularityScore = createCommand.PopularityScore,
                    ProductCode = createCommand.ProductCode,
                    Quantity = createCommand.Quantity,
                    Money = createCommand.Money,
                    ValidityPeriod = createCommand.ValidityPeriod,
                    WarehouseId = createCommand.WarehouseId,
                    SupplierId = createCommand.SupplierId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _repository.AddAsync(item as TEntity);

                // Log successful creation
                _logger.LogInformation($"Inventory item created: {item.Id}");
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error in HandleCreateInventoryItemAsync");

                // Rethrow or handle accordingly
                throw new CommandExecutionException("Error in CreateInventoryItemCommand");
            }
        }

        private async Task HandleUpdateInventoryItemAsync(UpdateInventoryItemCommand updateCommand)
        {
            try
            {
                var item = await _repository.GetByIdAsync(updateCommand.InventoryItemId) as InventoryItem;
                if (item != null)
                {
                    item.Name = updateCommand.NewName;
                    item.Description = updateCommand.NewDescription;
                    item.Category = updateCommand.NewCategory;
                    item.ProductCode = updateCommand.NewProductCode;
                    item.Quantity = updateCommand.NewQuantity;
                    item.Money = updateCommand.NewMoney;
                    item.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(item as TEntity);

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

        private async Task HandleDeleteInventoryItemAsync(DeleteInventoryItemCommand deleteCommand)
        {
            try
            {
                var item = await _repository.GetByIdAsync(deleteCommand.Id) as InventoryItem;
                if (item != null)
                {
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
        private async Task HandleCreateSupplierAsync(CreateSupplierCommand createSupplierCommand)
        {
            try
            {
                var supplier = new Supplier
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createSupplierCommand.Name,
                    Phone = createSupplierCommand.Phone,
                    ContactEmail = createSupplierCommand.ContactEmail,
                    Address = createSupplierCommand.Address,
                    IsActive = true,
                    ValidityPeriod = createSupplierCommand.ValidityPeriod,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _repository.AddAsync(supplier as TEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling CreateSupplierCommand");
                throw;
            }
        }

        private async Task HandleUpdateSupplierAsync(UpdateSupplierCommand updateSupplierCommand)
        {
            try
            {
                var supplier = await _repository.GetByIdAsync(updateSupplierCommand.SupplierId) as Supplier;
                if (supplier != null)
                {
                    supplier.Name = updateSupplierCommand.NewName;
                    supplier.Phone = updateSupplierCommand.NewPhone;
                    supplier.ContactEmail = updateSupplierCommand.NewContactEmail;
                    supplier.Address = updateSupplierCommand.NewAddress;
                    supplier.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(supplier as TEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling UpdateSupplierCommand");
                throw;
            }
        }

        private async Task HandleDeleteSupplierAsync(DeleteSupplierCommand deleteSupplierCommand)
        {
            try
            {
                var supplier = await _repository.GetByIdAsync(deleteSupplierCommand.Id) as Supplier;
                if (supplier != null)
                {
                    await _repository.RemoveAsync(supplier.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling DeleteSupplierCommand");
                throw;
            }
        }

        private async Task HandleActivateSupplierAsync(ActivateSupplierCommand activateSupplierCommand)
        {
            try
            {
                var supplier = await _repository.GetByIdAsync(activateSupplierCommand.SupplierId) as Supplier;
                if (supplier != null)
                {
                    supplier.IsActive = true;
                    supplier.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(supplier as TEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ActivateSupplierCommand");
                throw;
            }
        }

        private async Task HandleDeactivateSupplierAsync(DeactivateSupplierCommand deactivateSupplierCommand)
        {
            try
            {
                var supplier = await _repository.GetByIdAsync(deactivateSupplierCommand.SupplierId) as Supplier;
                if (supplier != null)
                {
                    supplier.IsActive = false;
                    supplier.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(supplier as TEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling DeactivateSupplierCommand");
                throw;
            }
        }

        // Warehouse handlers
        private async Task HandleCreateWarehouseAsync(CreateWarehouseCommand createWarehouseCommand)
        {
            try
            {
                var warehouse = new Warehouse
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createWarehouseCommand.Name,
                    Phone = createWarehouseCommand.Phone,
                    Address = createWarehouseCommand.Address,
                    IsOpen = createWarehouseCommand.IsOpen,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _repository.AddAsync(warehouse as TEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling CreateWarehouseCommand");
                throw;
            }
        }

        private async Task HandleUpdateWarehouseAsync(UpdateWarehouseCommand updateWarehouseCommand)
        {
            try
            {
                var warehouse = await _repository.GetByIdAsync(updateWarehouseCommand.WarehouseId) as Warehouse;
                if (warehouse != null)
                {
                    warehouse.Name = updateWarehouseCommand.NewName;
                    warehouse.Phone = updateWarehouseCommand.NewPhone;
                    warehouse.Address = updateWarehouseCommand.NewAddress;
                    warehouse.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(warehouse as TEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling UpdateWarehouseCommand");
                throw;
            }
        }

        private async Task HandleDeleteWarehouseAsync(DeleteWarehouseCommand deleteWarehouseCommand)
        {
            try
            {
                var warehouse = await _repository.GetByIdAsync(deleteWarehouseCommand.WarehouseId) as Warehouse;
                if (warehouse != null)
                {
                    await _repository.RemoveAsync(warehouse.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling DeleteWarehouseCommand");
                throw;
            }
        }

        private async Task HandleCloseWarehouseAsync(CloseWarehouseCommand closeWarehouseCommand)
        {
            try
            {
                var warehouse = await _repository.GetByIdAsync(closeWarehouseCommand.WarehouseId) as Warehouse;
                if (warehouse != null)
                {
                    warehouse.IsOpen = false;
                    warehouse.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(warehouse as TEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling CloseWarehouseCommand");
                throw;
            }
        }

        private async Task HandleReopenWarehouseAsync(ReopenWarehouseCommand reopenWarehouseCommand)
        {
            try
            {
                var warehouse = await _repository.GetByIdAsync(reopenWarehouseCommand.WarehouseId) as Warehouse;
                if (warehouse != null)
                {
                    warehouse.IsOpen = true;
                    warehouse.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(warehouse as TEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ReopenWarehouseCommand");
                throw;
            }
        }

        private async Task HandleMoveWarehouseAsync(MoveWarehouseCommand moveWarehouseCommand)
        {
            try
            {
                var warehouse = await _repository.GetByIdAsync(moveWarehouseCommand.WarehouseId) as Warehouse;
                if (warehouse != null)
                {
                    warehouse.Address.Street = moveWarehouseCommand.NewAddress;
                    warehouse.Address.Region = moveWarehouseCommand.NewRegion;
                    warehouse.Address.City = moveWarehouseCommand.NewCity;
                    warehouse.Address.Country = moveWarehouseCommand.NewCountry;
                    warehouse.Address.PostalCode = moveWarehouseCommand.NewPostalCode;

                    warehouse.IsOpen = true;
                    warehouse.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(warehouse as TEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling MoveWarehouseCommand");
                throw;
            }
        }
    }
}