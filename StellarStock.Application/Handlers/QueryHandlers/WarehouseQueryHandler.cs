namespace StellarStock.Application.Handlers.QueryHandlers
{
    public class WarehouseQueryHandler<TResult, TQuery> : IWarehouseQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WarehouseQueryHandler<TResult, TQuery>> _logger;

        public WarehouseQueryHandler(IWarehouseRepository warehouseRepository, IMapper mapper, ILogger<WarehouseQueryHandler<TResult, TQuery>> logger)
        {
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(TQuery query)
        {
            switch (query)
            {
                case GetClosedWarehousesQuery getClosedWarehousesQuery:
                    await HandleGetClosedWarehousesQueryAsync(getClosedWarehousesQuery);
                    break;

                case GetOpenedWarehousesQuery getOpenedWarehousesQuery:
                    await HandleGetOpenedWarehousesQueryAsync(getOpenedWarehousesQuery);
                    break;
                case GetWarehouseByIdQuery getWarehouseByIdQuery:
                    await HandleGetWarehouseByIdQueryAsync(getWarehouseByIdQuery);
                    break;
                case GetWarehousesByCityQuery getWarehousesByCityQuery:
                    await HandleGetWarehousesByCityQueryAsync(getWarehousesByCityQuery);
                    break;
                case GetWarehousesByRegionQuery getWarehousesByRegionQuery:
                    await HandleGetWarehousesByRegionQueryAsync(getWarehousesByRegionQuery);
                    break;
                case GetWarehouseStockedItemsQuery getWarehouseStockedItemsQuery:
                    await HandleGetWarehouseStockedItemsQueryAsync(getWarehouseStockedItemsQuery);
                    break;
                default:
                    _logger.LogError($"Unsupported query type: {typeof(TQuery)}");
                    throw new ArgumentException($"Unsupported query type: {typeof(TResult)}");
            };
        }

        private async Task<TResult> HandleGetClosedWarehousesQueryAsync(GetClosedWarehousesQuery query)
        {
            try
            {
                var closedWarehouses = await _warehouseRepository.GetClosedWarehouses();
                return _mapper.Map<TResult>(closedWarehouses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleGetClosedWarehousesQueryAsync");
                throw;
            }
        }

        private async Task<TResult> HandleGetOpenedWarehousesQueryAsync(GetOpenedWarehousesQuery query)
        {
            try
            {
                var openedWarehouses = await _warehouseRepository.GetOpenedWarehouses();
                return _mapper.Map<TResult>(openedWarehouses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleGetOpenedWarehousesQueryAsync");
                throw;
            }
        }

        private async Task<TResult> HandleGetWarehouseByIdQueryAsync(GetWarehouseByIdQuery query)
        {
            try
            {
                var warehouse = await _warehouseRepository.GetByIdAsync(query.WarehouseId);
                return _mapper.Map<TResult>(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleGetWarehouseByIdQueryAsync");
                throw;
            }
        }

        private async Task<TResult> HandleGetWarehousesByCityQueryAsync(GetWarehousesByCityQuery query)
        {
            try
            {
                var warehousesByCity = await _warehouseRepository.GetWarehousesByCityAsync(query.CityName);
                return _mapper.Map<TResult>(warehousesByCity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleGetWarehousesByCityQueryAsync");
                throw;
            }
        }

        private async Task<TResult> HandleGetWarehousesByRegionQueryAsync(GetWarehousesByRegionQuery query)
        {
            try
            {
                var warehousesByRegion = await _warehouseRepository.GetWarehousesByRegionAsync(query.RegionName);
                return _mapper.Map<TResult>(warehousesByRegion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleGetWarehousesByRegionQueryAsync");
                throw;
            }
        }

        private async Task<TResult> HandleGetWarehouseStockedItemsQueryAsync(GetWarehouseStockedItemsQuery query)
        {
            try
            {
                var stockedItems = await _warehouseRepository.GetWarehouseStockedItems(query.WarehouseId);
                return _mapper.Map<TResult>(stockedItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HandleGetWarehouseStockedItemsQueryAsync");
                throw;
            }
        }
    }
}
