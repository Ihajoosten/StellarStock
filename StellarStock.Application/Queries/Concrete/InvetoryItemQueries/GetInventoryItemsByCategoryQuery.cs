namespace StellarStock.Application.Queries.Concrete.InvetoryItemQueries
{
    public class GetInventoryItemsByCategoryQuery<TResult> : IInventoryItemQuery<TResult>
    {
        public ItemCategory Category { get; set; }
    }
}
