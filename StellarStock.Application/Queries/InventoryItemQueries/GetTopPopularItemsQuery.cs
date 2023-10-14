namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetTopPopularItemsQuery : IQuery<InventoryItem>
    {
        public int Count { get; set; }
    }
}