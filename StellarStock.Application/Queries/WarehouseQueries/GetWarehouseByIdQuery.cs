namespace StellarStock.Application.Queries.WarehouseQueries
{
    public class GetWarehouseByIdQuery : IQuery<Warehouse>
    {
        public string WarehouseId { get; set; }
    }
}
