using StellarStock.Domain.ValueObjects;

namespace StellarStock.Application.Handlers.CommandHandlers
{
    public class InventoryItemCommandHandler<TCommand, TEntity> : IGenericCommandHandler<TCommand, TEntity> where TCommand : ICommand where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly ILogger<InventoryItemCommandHandler<TCommand, TEntity>> _logger;

        public InventoryItemCommandHandler(IGenericRepository<TEntity> repository, ILogger<InventoryItemCommandHandler<TCommand, TEntity>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(TCommand command)
        {
            switch (command)
            {
                // Inventory Item Commands
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
                // Create the inventory item...
                var dummyItem = new InventoryItem
                {
                    Name = command.Name,
                    Description = command.Description,
                    Category = command.Category,
                    PopularityScore = command.PopularityScore,
                    ProductCode = command.ProductCode,
                    Quantity = command.Quantity,
                    Money = command.Money,
                    ValidityPeriod = command.ValidityPeriod,
                    WarehouseId = command.WarehouseId,
                    SupplierId = command.SupplierId
                };

                var inventoryAggregate = new InventoryAggregate(dummyItem);
                inventoryAggregate?.CreateInventoryItem(
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
                _logger.LogError($"Error in HandleCreateInventoryItemAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CreateInventoryItemCommand :: ${ex.Message}");
            }
        }

        private async Task HandleUpdateInventoryItemAsync(UpdateInventoryItemCommand command)
        {
            try
            {
                if (await _repository.GetByIdAsync(command.InventoryItemId) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);
                    itemAggregate?.UpdateItem(
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
                _logger.LogError($"Error in HandleUpdateInventoryItemAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in UpdateInventoryItemCommand :: ${ex.Message}");
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

                    itemAggregate?.RemoveItem();

                    await _repository.RemoveAsync(item.Id);

                    // Log successful deletion
                    _logger.LogInformation($"Inventory item deleted: {item.Id}");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleDeleteInventoryItemAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in DeleteInventoryItemCommand :: ${ex.Message}");
            }
        }
    }
}
