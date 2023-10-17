namespace StellarStock.Application.Queries.Concrete.WarehouseQueries
{
    public class GetWarehousesByRegionQuery<TResult> : IWarehouseQuery<TResult>
    {
        public string Region { get; set; }
    }
}
