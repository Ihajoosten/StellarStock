namespace StellarStock.Application.Queries.Concrete.SupplierQueries
{
    public class GetSupplierByIdQuery<TResult> : ISupplierQuery<TResult>
    {
        public string Id { get; set; }
    }
}
