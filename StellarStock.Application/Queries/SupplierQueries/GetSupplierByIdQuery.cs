namespace StellarStock.Application.Queries.SupplierQueries
{
    public class GetSupplierByIdQuery : IQuery<Supplier>
    {
        public string SupplierId { get; set; }
    }
}
