namespace StellarStock.Application.Queries.Handlers
{
    public class SupplierQueryHandler<TQuery, TResult> : ISupplierQueryHandler<TQuery, TResult>
    where TQuery : ISupplierQuery<TResult>
    {
        private readonly ISupplierRepository _repository;
        private readonly ILogger<SupplierQueryHandler<TQuery, TResult>> _logger;
        private readonly IMapper _mapper;

        public SupplierQueryHandler(ISupplierRepository repository, ILogger<SupplierQueryHandler<TQuery, TResult>> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            return query switch
            {
                GetActiveSuppliersQuery<TResult> => await HandleGetActiveAsync(query),
                GetSupplierByIdQuery<TResult> => await HandleGetByIdAsync(query),
                GetSuppliersByCityQuery<TResult> => await HandleGetByCityAsync(query),
                GetSuppliersByRegionQuery<TResult> => await HandleGetByRegionAsync(query),
                GetSuppliersWithValidityExpiringSoonQuery<TResult> => await HandleGetExpiringSoonAsync(query),
                _ => throw new ArgumentException($"Unsupported query type: {typeof(TQuery)}")
            };
        }

        public async Task<TResult> HandleGetActiveAsync(TQuery query)
        {
            try
            {
                var suppliers = await _repository.GetActiveSuppliersAsync();
                var result = _mapper.Map<TResult>(suppliers);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetActiveSuppliersAsync Supplier :: {ex.Message}");
                throw new Exception($"Error in HandleGetActiveSuppliersAsync Supplier :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByIdAsync(TQuery query)
        {
            try
            {
                var id = (query as GetSupplierByIdQuery<TResult>)?.Id;
                if (id != null)
                {
                    var supplier = await _repository.GetByIdAsync(id);
                    var result = _mapper.Map<TResult>(supplier);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetByIdAsync Supplier :: {ex.Message}");
                throw new Exception($"Error in HandleGetByIdAsync Supplier :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByCityAsync(TQuery query)
        {
            try
            {
                var city = (query as GetSuppliersByCityQuery<TResult>)?.City;
                if (!string.IsNullOrEmpty(city))
                {
                    var suppliers = await _repository.GetSuppliersByCityAsync(city);
                    var result = _mapper.Map<TResult>(suppliers);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetSuppliersByCityAsync Supplier :: {ex.Message}");
                throw new Exception($"Error in HandleGetSuppliersByCityAsync Supplier :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetByRegionAsync(TQuery query)
        {
            try
            {
                var region = (query as GetSuppliersByRegionQuery<TResult>)?.Region;
                if (!string.IsNullOrEmpty(region))
                {
                    var suppliers = await _repository.GetSuppliersByRegionAsync(region);
                    var result = _mapper.Map<TResult>(suppliers);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetSuppliersByRegionAsync Supplier :: {ex.Message}");
                throw new Exception($"Error in HandleGetSuppliersByRegionAsync Supplier :: {ex.Message}");
            }
        }

        public async Task<TResult> HandleGetExpiringSoonAsync(TQuery query)
        {
            try
            {
                var date = (query as GetInventoryItemsWithValidityExpiringSoonQuery<TResult>)?.ExpirationDate;
                if (!(date!.Value < DateTime.UtcNow))
                {
                    var suppliers = await _repository.GetSuppliersWithValidityExpiringSoonAsync(date.Value);
                    var result = _mapper.Map<TResult>(suppliers);
                    return result;
                }
                throw new ArgumentException($"Invalid query: {typeof(TQuery)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in HandleGetSuppliersWithValidityExpiringSoonAsync Supplier :: {ex.Message}");
                throw new Exception($"Error in HandleGetSuppliersWithValidityExpiringSoonAsync Supplier :: {ex.Message}");
            }
        }
    }

}
