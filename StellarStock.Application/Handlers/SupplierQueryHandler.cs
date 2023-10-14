namespace StellarStock.Application.Handlers
{
    public class SupplierQueryHandler<TResult, TQuery> : ISupplierQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IMapper _mapper;
        private readonly ISupplierRepository _supplierRepository;

        public SupplierQueryHandler(IMapper mapper, ISupplierRepository supplierRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _supplierRepository = supplierRepository ?? throw new ArgumentNullException(nameof(supplierRepository));
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            return query switch
            {
                GetActiveSuppliersQuery getActiveSuppliersQuery => await HandleGetActiveSuppliersQueryAsync(getActiveSuppliersQuery),
                GetSupplierByIdQuery getSuppliersByIdQuery => await HandleGetSuppliersByIdQueryAsync(getSuppliersByIdQuery),
                GetSuppliersByCityQuery getSuppliersByCityQuery => await HandleGetSuppliersByCityQueryAsync(getSuppliersByCityQuery),
                GetSuppliersByRegionQuery getSuppliersByRegionQuery => await HandleGetSuppliersByRegionQueryAsync(getSuppliersByRegionQuery),
                GetSuppliersWithValidityExpiringSoonQuery getSuppliersExpiringSoonQuery => await HandleGetSuppliersExpiringSoonQueryAsync(getSuppliersExpiringSoonQuery),
                _ => throw new ArgumentException($"Unsupported query type: {query.GetType().Name}"),
            };
        }

        private async Task<TResult> HandleGetActiveSuppliersQueryAsync(GetActiveSuppliersQuery query)
        {
            var activeSuppliers = await _supplierRepository.GetActiveSuppliersAsync();
            return _mapper.Map<TResult>(activeSuppliers);
        }

        private async Task<TResult> HandleGetSuppliersByIdQueryAsync(GetSupplierByIdQuery query)
        {
            var supplier = await _supplierRepository.GetByIdAsync(query.SupplierId);
            return _mapper.Map<TResult>(supplier);
        }

        private async Task<TResult> HandleGetSuppliersByCityQueryAsync(GetSuppliersByCityQuery query)
        {
            var suppliersByCity = await _supplierRepository.GetSuppliersByCityAsync(query.CityName);
            return _mapper.Map<TResult>(suppliersByCity);
        }

        private async Task<TResult> HandleGetSuppliersByRegionQueryAsync(GetSuppliersByRegionQuery query)
        {
            var suppliersByRegion = await _supplierRepository.GetSuppliersByRegionAsync(query.RegionName);
            return _mapper.Map<TResult>(suppliersByRegion);
        }

        private async Task<TResult> HandleGetSuppliersExpiringSoonQueryAsync(GetSuppliersWithValidityExpiringSoonQuery query)
        {
            var suppliersExpiringSoon = await _supplierRepository.GetSuppliersWithValidityExpiringSoonAsync(query.ExpirationDate);
            return _mapper.Map<TResult>(suppliersExpiringSoon);
        }
    }
}
