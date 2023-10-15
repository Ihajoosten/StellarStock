namespace StellarStock.Application.Handlers
{
    public class InventoryItemQueryHandler<TResult, TQuery> : IInventoryItemQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IMapper _mapper;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly ILogger<InventoryItemQueryHandler<TResult, TQuery>> _logger;

        public InventoryItemQueryHandler(IMapper mapper, IInventoryItemRepository inventoryItemRepository, ILogger<InventoryItemQueryHandler<TResult, TQuery>> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _inventoryItemRepository = inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(TQuery query)
        {
            switch (query)
            {
                case GetInventoryItemByIdQuery getByIdQuery:
                    await HandleGetInventoryItemByIdQueryAsync(getByIdQuery);
                    break;
                case GetTopPopularItemsQuery getTopPopularItemsQuery:
                    await HandleGetTopPopularItemsQueryAsync(getTopPopularItemsQuery);
                    break;
                case GetLowStockItemsQuery getLowStockItemsQuery:
                    await HandleGetLowStockItemsQueryAsync(getLowStockItemsQuery);
                    break;
                case GetInventoryItemsWithValidityExpiringSoonQuery getItemsExpiringSoonQuery:
                    await HandleGetItemsExpiringSoonQueryAsync(getItemsExpiringSoonQuery);
                    break;
                case GetInStockItemsQuery: 
                    await HandleGetItemsInStockQueryAsync(); 
                    break;
                case SearchInventoryItemsQuery searchItemsQuery:
                    await HandleSearchItemsQueryAsync(searchItemsQuery); 
                        break;
                case GetInventoryItemsByWarehouseQuery getByWarehouseQuery:
                    await HandleGetItemsByWarehouseQueryAsync(getByWarehouseQuery);
                        break;
                case GetInventoryItemsByCategoryQuery getByCategoryQuery:
                    await HandleGetByCategoryQueryAsync(getByCategoryQuery);
                    break;
                default:
                    _logger.LogError($"Unsupported query type: {typeof(TQuery)}");
                    throw new ArgumentException($"Unsupported query type: {typeof(TQuery)}");
            };
        }

        private async Task<TResult> HandleGetInventoryItemByIdQueryAsync(GetInventoryItemByIdQuery getInventoryItemByIdQuery)
        {
            try
            {
                var item = await _inventoryItemRepository.GetByIdAsync(getInventoryItemByIdQuery.InventoryItemId);
                return _mapper.Map<TResult>(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetInventoryItemByIdQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetTopPopularItemsQueryAsync(GetTopPopularItemsQuery getTopPopularItemsQuery)
        {
            try
            {
                var items = await _inventoryItemRepository.GetItemsByPopularityScoreAsync(getTopPopularItemsQuery.MinScore, getTopPopularItemsQuery.MaxScore);
                return _mapper.Map<TResult>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetTopPopularItemsQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetLowStockItemsQueryAsync(GetLowStockItemsQuery getLowStockItemsQuery)
        {
            try
            {
                var items = await _inventoryItemRepository.GetLowStockItemsAsync(getLowStockItemsQuery.Threshold);
                return _mapper.Map<TResult>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetLowStockItemsQuery");
                throw;
            }
        }

        private async Task<TResult> HandleSearchItemsQueryAsync(SearchInventoryItemsQuery searchItemsQuery)
        {
            try
            {
                var items = await _inventoryItemRepository.SearchItemsAsync(searchItemsQuery.Keyword);
                return _mapper.Map<TResult>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling SearchItemsQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetItemsByWarehouseQueryAsync(GetInventoryItemsByWarehouseQuery getItemsByWarehouseQuery)
        {
            try
            {
                var items = await _inventoryItemRepository.GetItemsByWarehouse(getItemsByWarehouseQuery.WarehouseId);
                return _mapper.Map<TResult>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetItemsByWarehouseQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetItemsExpiringSoonQueryAsync(GetInventoryItemsWithValidityExpiringSoonQuery getItemsExpiringSoonQuery)
        {
            try
            {
                var items = await _inventoryItemRepository.GetItemsExpiringSoonAsync(getItemsExpiringSoonQuery.ExpirationDate);
                return _mapper.Map<TResult>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetItemsExpiringSoonQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetByCategoryQueryAsync(GetInventoryItemsByCategoryQuery getByCategoryQuery)
        {
            try
            {
                var items = await _inventoryItemRepository.GetByCategoryAsync(getByCategoryQuery.Category);
                return _mapper.Map<TResult>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetByCategoryQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetItemsInStockQueryAsync()
        {
            try
            {
                var itemsInStock = await _inventoryItemRepository.GetItemsInStockAsync();
                return _mapper.Map<TResult>(itemsInStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetItemsInStockQuery");
                throw;
            }
        }
    }
}