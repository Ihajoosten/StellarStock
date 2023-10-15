namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetInventoryItemsByCategoryQuery : IQuery<InventoryItem>
    {
        public ItemCategory Category { get; set; }
    }
}