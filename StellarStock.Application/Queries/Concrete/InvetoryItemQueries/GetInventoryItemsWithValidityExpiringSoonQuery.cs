namespace StellarStock.Application.Queries.Concrete.InvetoryItemQueries
{
    public class GetInventoryItemsWithValidityExpiringSoonQuery<TResult> : IInventoryItemQuery<TResult>
    {
        public DateTime ExpirationDate { get; set; }
    }
}
