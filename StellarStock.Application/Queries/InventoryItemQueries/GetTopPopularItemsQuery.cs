namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetTopPopularItemsQuery : IQuery<InventoryItem>
    {
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
    }
}