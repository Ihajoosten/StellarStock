namespace StellarStock.Application.Queries.Concrete.WarehouseQueries
{
    public class GetWarehouseByIdQuery<TResult> : IWarehouseQuery<TResult>
    {
        public string Id { get; set; }
    }
}