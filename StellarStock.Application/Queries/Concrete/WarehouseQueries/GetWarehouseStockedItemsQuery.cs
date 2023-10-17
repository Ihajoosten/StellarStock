namespace StellarStock.Application.Queries.Concrete.WarehouseQueries
{
    public class GetWarehouseStockedItemsQuery<TResult> : IWarehouseQuery<TResult>
    {
        public string Id { get; set; }
    }
}
