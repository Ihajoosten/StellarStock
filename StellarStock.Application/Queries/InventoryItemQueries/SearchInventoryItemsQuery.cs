namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class SearchInventoryItemsQuery : IQuery<InventoryItem>
    {
        public string Keyword { get; set; }
    }
}