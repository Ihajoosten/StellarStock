namespace StellarStock.Application.Queries.Concrete.SupplierQueries
{
    public class GetSuppliersByRegionQuery<TResult> : ISupplierQuery<TResult>
    {
        public string Region { get; set; }
    }
}
