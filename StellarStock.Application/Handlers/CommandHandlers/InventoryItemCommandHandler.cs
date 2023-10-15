using StellarStock.Domain.Aggregates;
using StellarStock.Domain.Entities;
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

        public async Task<string> HandleAsync(TCommand command)
        {
            return command switch
            {
                // Inventory Item Commands
                CreateInventoryItemCommand createInventoryItemCommand => await HandleCreateInventoryItemAsync(createInventoryItemCommand),
                UpdateInventoryItemCommand updateInventoryItemCommand => await HandleUpdateInventoryItemAsync(updateInventoryItemCommand),
                DeleteInventoryItemCommand deleteInventoryItemCommand => await HandleDeleteInventoryItemAsync(deleteInventoryItemCommand),
                _ => LogAndThrowUnsupportedCommand(),
            };
        }

        private string LogAndThrowUnsupportedCommand()
        {
            _logger.LogError($"Unsupported command type: {typeof(TCommand)}");
            throw new ArgumentException($"Unsupported command type: {typeof(TCommand)}");
        }

        // Inventory Item handlers
        private async Task<string> HandleCreateInventoryItemAsync(CreateInventoryItemCommand command)
        {
            try
            {
                var inventoryAggregate = new InventoryAggregate(null);
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
                return inventoryAggregate.InventoryItem.Id!;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleCreateInventoryItemAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in CreateInventoryItemCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleUpdateInventoryItemAsync(UpdateInventoryItemCommand command)
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
                    _logger.LogInformation($"Inventory Item updated: {item.Id}");
                    return item.Id!;
                }

                // Log failed update
                _logger.LogInformation($"Inventory Item updated failed: {command.InventoryItemId}");
                return $"Unable to update Inventory Item: ${command.InventoryItemId}";
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError($"Error in HandleUpdateInventoryItemAsync :: ${ex.Message}");

                // Rethrow or handle accordingly
                throw new CommandExecutionException($"Error in UpdateInventoryItemCommand :: ${ex.Message}");
            }
        }

        private async Task<string> HandleDeleteInventoryItemAsync(DeleteInventoryItemCommand command)
        {
            try
            {
                //var item = await _repository.GetByIdAsync(command.Id) as InventoryItem;
                if (await _repository.GetByIdAsync(command.Id) is InventoryItem item)
                {
                    var itemAggregate = new InventoryAggregate(item);

                    itemAggregate?.RemoveItem();

                    await _repository.RemoveAsync(item.Id);

                    // Log successful removal
                    _logger.LogInformation($"Inventory Item updated: {item.Id}");
                    return item.Id!;
                }

                // Log failed removal
                _logger.LogInformation($"Inventory Item updated failed: {command.Id}");
                return $"Unable to update Inventory Item: ${command.Id}";
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
