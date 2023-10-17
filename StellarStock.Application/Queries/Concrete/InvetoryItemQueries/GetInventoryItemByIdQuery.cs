namespace StellarStock.Application.Queries.Concrete.InvetoryItemQueries
{
    public class GetInventoryItemByIdQuery<TResult> : IInventoryItemQuery<TResult>
    {
        public string Id { get; set; }
    }
}
