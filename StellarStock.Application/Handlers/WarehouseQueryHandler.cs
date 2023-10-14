namespace StellarStock.Application.Handlers
{
    public class WarehouseQueryHandler<TResult, TQuery> : IWarehouseQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public WarehouseQueryHandler(IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            return query switch
            {
                GetClosedWarehousesQuery getClosedWarehousesQuery => await HandleGetClosedWarehousesQueryAsync(getClosedWarehousesQuery),
                GetOpenedWarehousesQuery getOpenedWarehousesQuery => await HandleGetOpenedWarehousesQueryAsync(getOpenedWarehousesQuery),
                GetWarehouseByIdQuery getWarehouseByIdQuery => await HandleGetWarehouseByIdQueryAsync(getWarehouseByIdQuery),
                GetWarehousesByCityQuery getWarehousesByCityQuery => await HandleGetWarehousesByCityQueryAsync(getWarehousesByCityQuery),
                GetWarehousesByRegionQuery getWarehousesByRegionQuery => await HandleGetWarehousesByRegionQueryAsync(getWarehousesByRegionQuery),
                GetWarehouseStockedItemsQuery getWarehouseStockedItemsQuery => await HandleGetWarehouseStockedItemsQueryAsync(getWarehouseStockedItemsQuery),
                _ => throw new ArgumentException($"Unsupported query type: {query.GetType().Name}"),
            };
        }

        private async Task<TResult> HandleGetClosedWarehousesQueryAsync(GetClosedWarehousesQuery query)
        {
            var closedWarehouses = await _warehouseRepository.GetClosedWarehouses();
            return _mapper.Map<TResult>(closedWarehouses);
        }

        private async Task<TResult> HandleGetOpenedWarehousesQueryAsync(GetOpenedWarehousesQuery query)
        {
            var openedWarehouses = await _warehouseRepository.GetOpenedWarehouses();
            return _mapper.Map<TResult>(openedWarehouses);
        }

        private async Task<TResult> HandleGetWarehouseByIdQueryAsync(GetWarehouseByIdQuery query)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(query.WarehouseId);
            return _mapper.Map<TResult>(warehouse);
        }

        private async Task<TResult> HandleGetWarehousesByCityQueryAsync(GetWarehousesByCityQuery query)
        {
            var warehousesByCity = await _warehouseRepository.GetWarehousesByCityAsync(query.CityName);
            return _mapper.Map<TResult>(warehousesByCity);
        }

        private async Task<TResult> HandleGetWarehousesByRegionQueryAsync(GetWarehousesByRegionQuery query)
        {
            var warehousesByRegion = await _warehouseRepository.GetWarehousesByRegionAsync(query.RegionName);
            return _mapper.Map<TResult>(warehousesByRegion);
        }

        private async Task<TResult> HandleGetWarehouseStockedItemsQueryAsync(GetWarehouseStockedItemsQuery query)
        {
            var stockedItems = await _warehouseRepository.GetWarehouseStockedItems(query.WarehouseId);
            return _mapper.Map<TResult>(stockedItems);
        }
    }
}
