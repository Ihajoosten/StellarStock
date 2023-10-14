namespace StellarStock.Application.Handlers
{
    public class InventoryItemQueryHandler<TResult, TQuery> : IInventoryItemQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IMapper _mapper;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryItemQueryHandler(IMapper mapper, IInventoryItemRepository inventoryItemRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _inventoryItemRepository = inventoryItemRepository ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            return query switch
            {
                GetInventoryItemByIdQuery getByIdQuery => await HandleGetInventoryItemByIdQueryAsync(getByIdQuery),
                GetTopPopularItemsQuery getTopPopularItemsQuery => await HandleGetTopPopularItemsQueryAsync(getTopPopularItemsQuery),
                GetLowStockItemsQuery getLowStockItemsQuery => await HandleGetLowStockItemsQueryAsync(getLowStockItemsQuery),
                GetInventoryItemsWithValidityExpiringSoonQuery getItemsExpiringSoonQuery => await HandleGetItemsExpiringSoonQueryAsync(getItemsExpiringSoonQuery),
                GetInStockItemsQuery => await HandleGetItemsInStockQueryAsync(),
                SearchInventoryItemsQuery searchItemsQuery => await HandleSearchItemsQueryAsync(searchItemsQuery),
                GetInventoryItemsByWarehouseQuery getByWarehouseQuery => await HandleGetItemsByWarehouseQueryAsync(getByWarehouseQuery),
                GetInventoryItemsByCategoryQuery getByCategoryQuery => await HandleGetByCategoryQueryAsync(getByCategoryQuery),
                _ => throw new ArgumentException($"Unsupported query type: {typeof(TQuery)}"),
            };
        }

        private async Task<TResult> HandleGetInventoryItemByIdQueryAsync(GetInventoryItemByIdQuery getInventoryItemByIdQuery)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(getInventoryItemByIdQuery.InventoryItemId);
            return _mapper.Map<TResult>(item);
        }
        private async Task<TResult> HandleGetTopPopularItemsQueryAsync(GetTopPopularItemsQuery getTopPopularItemsQuery)
        {
            var items = await _inventoryItemRepository.GetItemsByPopularityScoreAsync(getTopPopularItemsQuery.MinScore, getTopPopularItemsQuery.MaxScore);
            return _mapper.Map<TResult>(items);
        }
        private async Task<TResult> HandleGetLowStockItemsQueryAsync(GetLowStockItemsQuery getLowStockItemsQuery)
        {
            var items = await _inventoryItemRepository.GetLowStockItemsAsync(getLowStockItemsQuery.Threshold);
            return _mapper.Map<TResult>(items);
        }
        private async Task<TResult> HandleSearchItemsQueryAsync(SearchInventoryItemsQuery searchItemsQuery)
        {
            var items = await _inventoryItemRepository.SearchItemsAsync(searchItemsQuery.Keyword);
            return _mapper.Map<TResult>(items);
        }
        private async Task<TResult> HandleGetItemsByWarehouseQueryAsync(GetInventoryItemsByWarehouseQuery getItemsByWarehouseQuery)
        {
            var items = await _inventoryItemRepository.GetItemsByWarehouse(getItemsByWarehouseQuery.WarehouseId);
            return _mapper.Map<TResult>(items);
        }
        private async Task<TResult> HandleGetItemsExpiringSoonQueryAsync(GetInventoryItemsWithValidityExpiringSoonQuery getItemsExpiringSoonQuery)
        {
            var items = await _inventoryItemRepository.GetItemsExpiringSoonAsync(getItemsExpiringSoonQuery.ExpirationDate);
            return _mapper.Map<TResult>(items);
        }
        private async Task<TResult> HandleGetByCategoryQueryAsync(GetInventoryItemsByCategoryQuery getByCategoryQuery)
        {
            var items = await _inventoryItemRepository.GetByCategoryAsync(getByCategoryQuery.Category);
            return _mapper.Map<TResult>(items);
        }
        private async Task<TResult> HandleGetItemsInStockQueryAsync()
        {
            var itemsInStock = await _inventoryItemRepository.GetItemsInStockAsync();
            return _mapper.Map<TResult>(itemsInStock);
        }
    }
}