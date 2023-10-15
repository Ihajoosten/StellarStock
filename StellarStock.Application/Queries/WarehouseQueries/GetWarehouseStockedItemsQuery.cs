namespace StellarStock.Application.Queries.WarehouseQueries
{
    public class GetWarehouseStockedItemsQuery : IQuery<Warehouse>
    {
        public string WarehouseId { get; set; }
    }
}
