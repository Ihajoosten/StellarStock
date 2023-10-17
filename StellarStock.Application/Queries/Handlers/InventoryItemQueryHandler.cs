namespace StellarStock.Application.Queries.Handlers
{
    public class InventoryItemQueryHandler<TQuery, TResult> : IInventoryItemQueryHandler<TQuery, TResult>
     where TQuery : IInventoryItemQuery<TResult>
    {
        private readonly IInventoryItemRepository _repository;
        private readonly ILogger<InventoryItemQueryHandler<TQuery, TResult>> _logger;
        private readonly IMapper _mapper;

        public InventoryItemQueryHandler(IInventoryItemRepository repository, ILogger<InventoryItemQueryHandler<TQuery, TResult>> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            return query switch
            {
                GetInStockItemsQuery<TResult> => await HandleGetInStockAsync(query),
                GetInventoryItemByIdQuery<TResult> => await HandleGetByIdAsync(query),
                GetInventoryItemsByCategoryQuery<TResult> => await HandleGetByCategoryAsync(query),
                GetInventoryItemsByWarehouseQuery<TResult> => await HandleGetByWarehouseAsync(query),
                GetInventoryItemsWithValidityExpiringSoonQuery<TResult> => await HandleGetExpiringSoonAsync(query),
                GetLowStockItemsQuery<TResult> => await HandleGetLowStockAsync(query),
                GetTopPopularItemsQuery<TResult> => await HandleGetItemsByPopularityScoreAsync(query),
                SearchInventoryItemsQuery<TResult> => await HandleSearchAsync(query),
                _ => throw new ArgumentException($"Unsupported query type: {typeof(TQuery)}")
            };
        }

        public async Task<TResult> HandleGetInStockAsync(TQuery query)
        {
            try
            {
                var inStockItems = await _repository.GetItemsInStockAsync();
                var result = _mapper.Map<TResult>(inStockItems);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetInStockAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleGetInStockAsync InventoryItem :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByIdAsync(TQuery query)
        {
            try
            {
                var id = (query as GetInventoryItemByIdQuery<TResult>)?.Id;
                if (id != null)
                {
                    var inventoryItem = await _repository.GetByIdAsync(id);
                    var result = _mapper.Map<TResult>(inventoryItem);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetByIdAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleGetByIdAsync InventoryItem :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByCategoryAsync(TQuery query)
        {
            try
            {
                var category = (query as GetInventoryItemsByCategoryQuery<TResult>)?.Category;
                if (category!.Value.Equals(typeof(ItemCategory)))
                {
                    var inventoryItems = await _repository.GetByCategoryAsync(category.Value);
                    var result = _mapper.Map<TResult>(inventoryItems);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetByCategoryAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleGetByCategoryAsync InventoryItem :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByWarehouseAsync(TQuery query)
        {
            try
            {
                var warehouseId = (query as GetInventoryItemsByWarehouseQuery<TResult>)?.Id;
                if (!string.IsNullOrEmpty(warehouseId))
                {
                    var inventoryItems = await _repository.GetItemsByWarehouse(warehouseId);
                    var result = _mapper.Map<TResult>(inventoryItems);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetByWarehouseAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleGetByWarehouseAsync InventoryItem :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetExpiringSoonAsync(TQuery query)
        {
            try
            {
                var date = (query as GetInventoryItemsWithValidityExpiringSoonQuery<TResult>)?.ExpirationDate;
                if (!(date!.Value < DateTime.UtcNow))
                {
                    var expiringSoonItems = await _repository.GetItemsExpiringSoonAsync(date.Value);
                    var result = _mapper.Map<TResult>(expiringSoonItems);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetExpiringSoonAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleGetExpiringSoonAsync InventoryItem :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetLowStockAsync(TQuery query)
        {
            try
            {
                var treshold = (query as GetLowStockItemsQuery<TResult>)?.Threshold;
                if (int.IsPositive(treshold.Value))
                {
                    var lowStockItems = await _repository.GetLowStockItemsAsync(treshold.Value);
                    var result = _mapper.Map<TResult>(lowStockItems);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetLowStockAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleGetLowStockAsync InventoryItem :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetItemsByPopularityScoreAsync(TQuery query)
        {
            try
            {
                var minScore = (query as GetTopPopularItemsQuery<TResult>)?.MinScore;
                var maxScore = (query as GetTopPopularItemsQuery<TResult>)?.MaxScore;

                if (int.IsPositive(minScore!.Value) && int.IsPositive(maxScore!.Value))
                {
                    var topPopularItems = await _repository.GetItemsByPopularityScoreAsync(minScore.Value, maxScore.Value);
                    var result = _mapper.Map<TResult>(topPopularItems);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetTopPopularAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleGetTopPopularAsync InventoryItem :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleSearchAsync(TQuery query)
        {
            try
            {
                var searchQuery = (query as SearchInventoryItemsQuery<TResult>)?.Keyword;
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchResults = await _repository.SearchItemsAsync(searchQuery);
                    var result = _mapper.Map<TResult>(searchResults);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleSearchAsync InventoryItem :: {ex.Message}");
                throw new Exception($"Error in HandleSearchAsync InventoryItem :: {ex.Message}");
            }
        }
    }
}
