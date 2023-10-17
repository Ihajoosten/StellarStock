namespace StellarStock.Application.Queries.Concrete.InvetoryItemQueries
{
    public class GetLowStockItemsQuery<TResult> : IInventoryItemQuery<TResult>
    {
        public int Threshold { get; set; }
    }
}
