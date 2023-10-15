namespace StellarStock.Application.Queries.Concrete.SupplierQueries
{
    public class GetSuppliersWithValidityExpiringSoonQuery<TResult> : ISupplierQuery<TResult>
    {
        public DateTime ExpirationDate { get; set; }
    }
}
