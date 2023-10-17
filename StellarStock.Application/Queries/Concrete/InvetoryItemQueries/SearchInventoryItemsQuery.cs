namespace StellarStock.Application.Queries.Concrete.InvetoryItemQueries
{
    public class SearchInventoryItemsQuery<TResult> : IInventoryItemQuery<TResult>
    {
        public string Keyword { get; set; }
    }
}
