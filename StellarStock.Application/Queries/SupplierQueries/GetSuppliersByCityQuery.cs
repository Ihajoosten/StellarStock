namespace StellarStock.Application.Queries.SupplierQueries
{
    public class GetSuppliersByCityQuery : IQuery<Supplier>
    {
        public string CityName { get; set; }
    }
}
