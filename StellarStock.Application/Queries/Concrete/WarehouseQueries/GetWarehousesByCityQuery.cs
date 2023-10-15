namespace StellarStock.Application.Queries.Concrete.WarehouseQueries
{
    public class GetWarehousesByCityQuery<TResult> : IWarehouseQuery<TResult>
    {
        public string City { get; set; }
    }
}