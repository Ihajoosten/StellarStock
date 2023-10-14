namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetInventoryItemsWithValidityExpiringSoonQuery : IQuery<InventoryItem>
    {
        public DateTime ExpirationDate { get; set; }
    }
}
