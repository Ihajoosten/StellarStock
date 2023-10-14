namespace StellarStock.Application.Queries.SupplierQueries
{
    public class GetSuppliersWithValidityExpiringSoonQuery : IQuery<Supplier>
    {
        public DateTime ExpirationDate { get; set; }
    }
}
