namespace StellarStock.Application.Queries.Concrete.InvetoryItemQueries
{
    public class GetInventoryItemsByWarehouseQuery<TResult> : IInventoryItemQuery<TResult>
    {
        public string Id { get; set; }
    }
}
