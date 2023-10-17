namespace StellarStock.Application.Queries.Handlers
{
    public class WarehouseQueryHandler<TQuery, TResult> : IWarehouseQueryHandler<TQuery, TResult>
    where TQuery : IWarehouseQuery<TResult>
    {
        private readonly IWarehouseRepository _repository;
        private readonly ILogger<WarehouseQueryHandler<TQuery, TResult>> _logger;
        private readonly IMapper _mapper;

        public WarehouseQueryHandler(IWarehouseRepository repository, ILogger<WarehouseQueryHandler<TQuery, TResult>> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            return query switch
            {
                GetClosedWarehousesQuery<TResult> => await HandleGetClosedAsync(query),
                GetOpenedWarehousesQuery<TResult> => await HandleGetOpenedAsync(query),
                GetWarehouseByIdQuery<TResult> => await HandleGetByIdAsync(query),
                GetWarehouseStockedItemsQuery<TResult> => await HandleGetStockedItemsAsync(query),
                GetWarehousesByCityQuery<TResult> => await HandleGetByCityAsync(query),
                GetWarehousesByRegionQuery<TResult> => await HandleGetByRegionAsync(query),
                _ => throw new ArgumentException($"Unsupported query type: {typeof(TQuery)}")
            };
        }

        public async Task<TResult> HandleGetClosedAsync(TQuery query)
        {
            try
            {
                var warehouses = await _repository.GetClosedWarehouses();
                var result = _mapper.Map<TResult>(warehouses);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetClosedAsync Warehouse :: {ex.Message}");
                throw new Exception($"Error in HandleGetClosedAsync Warehouse :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetOpenedAsync(TQuery query)
        {
            try
            {
                var warehouses = await _repository.GetOpenedWarehouses();
                var result = _mapper.Map<TResult>(warehouses);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetOpenedAsync Warehouse :: {ex.Message}");
                throw new Exception($"Error in HandleGetOpenedAsync Warehouse :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByIdAsync(TQuery query)
        {
            try
            {
                var id = (query as GetWarehouseByIdQuery<TResult>)?.Id;
                if (id != null)
                {
                    var warehouse = await _repository.GetByIdAsync(id);
                    var result = _mapper.Map<TResult>(warehouse);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetByIdAsync Warehouse :: {ex.Message}");
                throw new Exception($"Error in HandleGetByIdAsync Warehouse :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetStockedItemsAsync(TQuery query)
        {
            try
            {
                var id = (query as GetWarehouseStockedItemsQuery<TResult>)?.Id;
                if (id != null)
                {
                    var warehouse = await _repository.GetByIdAsync(id);
                    if (warehouse != null)
                    {
                        var stockedItems = await _repository.GetWarehouseStockedItems(id);
                        var result = _mapper.Map<TResult>(stockedItems);
                        return result;
                    }
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetStockedItemsAsync Warehouse :: {ex.Message}");
                throw new Exception($"Error in HandleGetStockedItemsAsync Warehouse :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByCityAsync(TQuery query)
        {
            try
            {
                var city = (query as GetWarehousesByCityQuery<TResult>)?.City;
                if (!string.IsNullOrEmpty(city))
                {
                    var warehouses = await _repository.GetWarehousesByCityAsync(city);
                    var result = _mapper.Map<TResult>(warehouses);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetByCityAsync Warehouse :: {ex.Message}");
                throw new Exception($"Error in HandleGetByCityAsync Warehouse :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByRegionAsync(TQuery query)
        {
            try
            {
                var region = (query as GetWarehousesByRegionQuery<TResult>)?.Region;
                if (!string.IsNullOrEmpty(region))
                {
                    var warehouses = await _repository.GetWarehousesByRegionAsync(region);
                    var result = _mapper.Map<TResult>(warehouses);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetByRegionAsync Warehouse :: {ex.Message}");
                throw new Exception($"Error in HandleGetByRegionAsync Warehouse :: {ex.Message}");
            }
        }
    }
}