namespace StellarStock.Application.Queries.Concrete.InvetoryItemQueries
{
    public class GetTopPopularItemsQuery<TResult> : IInventoryItemQuery<TResult>
    {
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
    }
}
