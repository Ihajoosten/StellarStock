namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetInventoryItemByIdQuery : IQuery<InventoryItem>
    {
        public string InventoryItemId { get; set; }
    }
}