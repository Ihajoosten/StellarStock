namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetInventoryItemsByWarehouseQuery : IQuery<InventoryItem>
    {
        public string WarehouseId { get; set; }
    }
}