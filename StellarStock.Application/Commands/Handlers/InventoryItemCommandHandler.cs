namespace StellarStock.Application.Commands.Handlers
{
    public class InventoryItemCommandHandler<TCommand> : IInventoryItemCommandHandler<TCommand>
            where TCommand : IInventoryItemCommand
    {
        private readonly IGenericRepository<InventoryItem> _repository;
        private readonly ILogger<InventoryItemCommandHandler<TCommand>> _logger;

        public InventoryItemCommandHandler(IGenericRepository<InventoryItem> repository, ILogger<InventoryItemCommandHandler<TCommand>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Dictionary<string, bool>> HandleAsync(TCommand command)
        {
            return command switch
            {
                CreateInventoryItemCommand => await HandleCreateAsync(command),
                UpdateInventoryItemCommand => await HandleUpdateAsync(command),
                DeleteInventoryItemCommand => await HandleDeleteAsync(command),
                IncreaseInventoryItemQuantityCommand => await HandleIncreaseQuantityAsync(command),
                DecreaseInventoryItemQuantityCommand => await HandleDecreaseQuantityAsync(command),
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
                if (command is not CreateInventoryItemCommand createCommand)
                {
                    _logger.LogError("Invalid command type for HandleCreateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                var newItem = new InventoryItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = createCommand.Name,
                    Description = createCommand.Description,
                    Category = createCommand.Category,
                    PopularityScore = createCommand.PopularityScore,
                    ProductCode = createCommand.ProductCode,
                    Quantity = createCommand.Quantity,
                    Money = createCommand.Money,
                    WarehouseId = createCommand.WarehouseId,
                    SupplierId = createCommand.SupplierId,
                    ValidityPeriod = createCommand.ValidityPeriod,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var added = await _repository.AddAsync(newItem);

                if (added)
                {
                    _logger.LogInformation($"Inventory item created: {newItem.Id}");
                    return new Dictionary<string, bool> { { newItem.Id, true } };
                }
                else
                {
                    _logger.LogInformation($"Inventory item creation failed: {newItem.Id}");
                    return new Dictionary<string, bool> { { newItem.Id, false } };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleCreateAsync Inventory Item :: {ex.Message}");
                throw new Exception($"Error in HandleCreateAsync Inventory Item :: {ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleUpdateAsync(TCommand command)
        {
            try
            {
                if (command is not UpdateInventoryItemCommand updateCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleUpdateAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(updateCommand.Id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);
                    itemAggregate?.UpdateItem(
                        updateCommand.NewName,
                        updateCommand.NewDescription,
                        updateCommand.NewCategory,
                        updateCommand.NewProductCode,
                        updateCommand.NewPopularityScore,
                        updateCommand.NewQuantity,
                        updateCommand.NewMoney);

                    var updated = await _repository.UpdateAsync(itemAggregate!.InventoryItem);
                    if (updated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Inventory Item updated: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id!, true } };
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Inventory Item updated failed: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id, false } };
                    }
                }
                else
                {
                    // Log failed update
                    _logger.LogInformation($"Inventory Item updated failed: {updateCommand.Id}");
                    return new Dictionary<string, bool> { { updateCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateAsync Inventory Item :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleUpdateAsync Inventory Item :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleDeleteAsync(TCommand command)
        {
            try
            {
                if (command is not DeleteInventoryItemCommand deleteCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleDeleteAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(deleteCommand.Id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);
                    itemAggregate.RemoveItem();

                    var deleted = await _repository.RemoveAsync(item.Id!);
                    if (deleted)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Inventory Item deleted: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id!, true } };
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Inventory Item deleting failed: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id!, false } };
                    }
                }
                else
                {
                    // Log failed deletion
                    _logger.LogInformation($"Inventory Item deleting failed: {deleteCommand.Id}");
                    return new Dictionary<string, bool> { { deleteCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteAsync Inventory Item :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleDeleteAsync Inventory Item :: ${ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleIncreaseQuantityAsync(TCommand command)
        {
            try
            {
                if (command is not IncreaseInventoryItemQuantityCommand increaseCommand)
                {
                    // Log an error and return an appropriate result
                    _logger.LogError("Invalid command type for HandleIncreaseQuantityAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(increaseCommand.Id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);
                    itemAggregate?.UpdateQuantity(increaseCommand.Quantity);

                    var updated = await _repository.UpdateAsync(itemAggregate!.InventoryItem);

                    if (updated)
                    {
                        _logger.LogInformation($"Quantity updated from Inventory Item: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id!, true } };
                    }
                    else
                    {
                        _logger.LogInformation($"Quantity update failed from Inventory Item: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id!, false } };
                    }
                }
                else
                {
                    _logger.LogInformation($"Quantity update failed from Inventory Item: {increaseCommand.Id}");
                    return new Dictionary<string, bool> { { increaseCommand.Id, false } };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleIncreaseQuantityAsync Inventory Item :: {ex.Message}");
                throw new Exception($"Error in HandleIncreaseQuantityAsync Inventory Item :: {ex.Message}");
            }
        }

        public async Task<Dictionary<string, bool>> HandleDecreaseQuantityAsync(TCommand command)
        {
            try
            {
                if (command is not DecreaseInventoryItemQuantityCommand decreaseCommand)
                {
                    _logger.LogError("Invalid command type for HandleDecreaseQuantityAsync");
                    return new Dictionary<string, bool> { { "InvalidCommandType", false } };
                }

                if (await _repository.GetByIdAsync(decreaseCommand.Id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);
                    itemAggregate?.UpdateQuantity(decreaseCommand.Quantity);

                    var updated = await _repository.UpdateAsync(itemAggregate!.InventoryItem);

                    if (updated)
                    {
                        _logger.LogInformation($"Quantity updated from Inventory Item: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id!, true } };
                    }
                    else
                    {
                        _logger.LogInformation($"Quantity update failed from Inventory Item: {item.Id}");
                        return new Dictionary<string, bool> { { item.Id!, false } };
                    }
                }
                else
                {
                    _logger.LogInformation($"Quantity update failed from Inventory Item: {decreaseCommand.Id}");
                    return new Dictionary<string, bool> { { decreaseCommand.Id!, true } };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleDecreaseQuantityAsync Inventory Item :: {ex.Message}");
                throw new Exception($"Error in HandleDecreaseQuantityAsync Inventory Item :: {ex.Message}");
            }
        }
    }
}