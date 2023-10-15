namespace StellarStock.Application.Handlers.QueryHandlers
{
    public class SupplierQueryHandler<TResult, TQuery> : ISupplierQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly ILogger<SupplierQueryHandler<TResult, TQuery>> _logger;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierQueryHandler(ILogger<SupplierQueryHandler<TResult, TQuery>> logger, ISupplierRepository supplierRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task HandleAsync(TQuery query)
        {
            switch (query)
            {
                case GetActiveSuppliersQuery getActiveSuppliersQuery:
                    await HandleGetActiveSuppliersQueryAsync(getActiveSuppliersQuery);
                    break;
                case GetSupplierByIdQuery getSupplierByIdQuery:
                    await HandleGetSuppliersByIdQueryAsync(getSupplierByIdQuery);
                    break;
                case GetSuppliersByCityQuery getSuppliersByCityQuery:
                    await HandleGetSuppliersByCityQueryAsync(getSuppliersByCityQuery);
                    break;
                case GetSuppliersByRegionQuery getSuppliersByRegionQuery:
                    await HandleGetSuppliersByRegionQueryAsync(getSuppliersByRegionQuery);
                    break;
                case GetSuppliersWithValidityExpiringSoonQuery getSuppliersExpiringSoonQuery:
                    await HandleGetSuppliersExpiringSoonQueryAsync(getSuppliersExpiringSoonQuery);
                    break;
                default:
                    _logger.LogError($"Unsupported query type: {typeof(TQuery)}");
                    throw new ArgumentException($"Unsupported query type: {typeof(TResult)}");
            }
        }

        private async Task<TResult> HandleGetActiveSuppliersQueryAsync(GetActiveSuppliersQuery query)
        {
            try
            {
                var activeSuppliers = await _supplierRepository.GetActiveSuppliersAsync();
                return _mapper.Map<TResult>(activeSuppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetActiveSuppliersQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetSuppliersByIdQueryAsync(GetSupplierByIdQuery query)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(query.SupplierId);
                return _mapper.Map<TResult>(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetSuppliersByIdQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetSuppliersByCityQueryAsync(GetSuppliersByCityQuery query)
        {
            try
            {
                var suppliersByCity = await _supplierRepository.GetSuppliersByCityAsync(query.CityName);
                return _mapper.Map<TResult>(suppliersByCity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetSuppliersByCityQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetSuppliersByRegionQueryAsync(GetSuppliersByRegionQuery query)
        {
            try
            {
                var suppliersByRegion = await _supplierRepository.GetSuppliersByRegionAsync(query.RegionName);
                return _mapper.Map<TResult>(suppliersByRegion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetSuppliersByRegionQuery");
                throw;
            }
        }

        private async Task<TResult> HandleGetSuppliersExpiringSoonQueryAsync(GetSuppliersWithValidityExpiringSoonQuery query)
        {
            try
            {
                var suppliersExpiringSoon = await _supplierRepository.GetSuppliersWithValidityExpiringSoonAsync(query.ExpirationDate);
                return _mapper.Map<TResult>(suppliersExpiringSoon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetSuppliersExpiringSoonQuery");
                throw;
            }
        }
    }
}
