namespace StellarStock.Application.Queries.Concrete.SupplierQueries
{
    public class GetSuppliersByCityQuery<TResult> : ISupplierQuery<TResult>
    {
        public string City { get; set; }
    }
}
