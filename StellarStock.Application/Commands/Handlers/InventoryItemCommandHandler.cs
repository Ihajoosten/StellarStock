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

        public async Task<bool> HandleAsync(TCommand command)
        {
            return command switch
            {
                CreateInventoryItemCommand => await HandleCreateAsync(command),
                UpdateInventoryItemCommand => await HandleUpdateAsync(command),
                DeleteInventoryItemCommand => await HandleDeleteAsync(command),
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
                var name = (command as CreateInventoryItemCommand)!.Name;
                var description = (command as CreateInventoryItemCommand)!.Description;
                var category = (command as CreateInventoryItemCommand)!.Category;
                var popularityScore = (command as CreateInventoryItemCommand)!.PopularityScore;
                var productCode = (command as CreateInventoryItemCommand)!.ProductCode;
                var quantity = (command as CreateInventoryItemCommand)!.Quantity;
                var money = (command as CreateInventoryItemCommand)!.Money;
                var warehouseId = (command as CreateInventoryItemCommand)!.WarehouseId;
                var supplierId = (command as CreateInventoryItemCommand)!.SupplierId;
                var validityPeriod = (command as CreateInventoryItemCommand)!.ValidityPeriod;

                var inventoryAggregate = new InventoryAggregate(null);
                inventoryAggregate?.CreateInventoryItem(name, description, category, popularityScore,
                    productCode, quantity, money, warehouseId, supplierId, validityPeriod);

                var created = await _repository.AddAsync(inventoryAggregate.InventoryItem);

                if (created)
                {
                    // Log successful creation
                    _logger.LogInformation($"Inventory item created: {inventoryAggregate.InventoryItem.Id}");
                    return created;
                }
                else
                {
                    // Log failed update
                    _logger.LogInformation($"Inventory item creation failed: {inventoryAggregate.InventoryItem.Id}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateAsync Inventory Item :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleCreateAsync Inventory Item :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleUpdateAsync(TCommand command)
        {
            try
            {
                var id = (command as UpdateInventoryItemCommand)!.Id;
                var name = (command as UpdateInventoryItemCommand)!.NewName;
                var description = (command as UpdateInventoryItemCommand)!.NewDescription;
                var category = (command as UpdateInventoryItemCommand)!.NewCategory;
                var popularityScore = (command as UpdateInventoryItemCommand)!.NewPopularityScore;
                var productCode = (command as UpdateInventoryItemCommand)!.NewProductCode;
                var quantity = (command as UpdateInventoryItemCommand)!.NewQuantity;
                var money = (command as UpdateInventoryItemCommand)!.NewMoney;

                if (await _repository.GetByIdAsync(id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);
                    itemAggregate?.UpdateItem(name, description, category, productCode, popularityScore, quantity, money);

                    var updated = await _repository.UpdateAsync(itemAggregate.InventoryItem);

                    if (updated)
                    {
                        // Log successful update
                        _logger.LogInformation($"Inventory Item updated: {item.Id}");
                        return updated;
                    }
                    else
                    {
                        // Log failed update
                        _logger.LogInformation($"Inventory Item updated failed: {item.Id}");
                        return false;
                    }
                }

                // Log failed update
                _logger.LogInformation($"Inventory Item updated failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateAsync Inventory Item :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleUpdateAsync Inventory Item :: ${ex.Message}");
            }
        }

        public async Task<bool> HandleDeleteAsync(TCommand command)
        {
            try
            {
                var id = (command as UpdateInventoryItemCommand)!.Id;

                if (await _repository.GetByIdAsync(id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);

                    itemAggregate?.RemoveItem();

                    var deleted = await _repository.RemoveAsync(item.Id!);

                    if (deleted)
                    {
                        // Log successful deletion
                        _logger.LogInformation($"Inventory Item deleted: {item.Id}");
                        return deleted;
                    }
                    else
                    {
                        // Log failed deletion
                        _logger.LogInformation($"Inventory Item deleting failed: {item.Id}");
                        return false;
                    }
                }

                // Log failed deletion
                _logger.LogInformation($"Inventory Item deleting failed: {id}");
                return false;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteAsync Inventory Item :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new Exception($"Error in HandleDeleteAsync Inventory Item :: ${ex.Message}");
            }
        }
    }
}