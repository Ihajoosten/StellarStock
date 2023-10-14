namespace StellarStock.Application.Queries.SupplierQueries
{
    public class GetSuppliersByRegionQuery : IQuery<Supplier>
    {
        public string RegionName { get; set; }
    }
}
