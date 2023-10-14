namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetLowStockItemsQuery : IQuery<InventoryItem>
    {
        public int Threshold { get; set; }
    }
}