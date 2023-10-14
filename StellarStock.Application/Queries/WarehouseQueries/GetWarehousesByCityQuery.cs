namespace StellarStock.Application.Queries.WarehouseQueries
{
    public class GetWarehousesByCityQuery : IQuery<Warehouse>
    {
        public string CityName { get; set; }
    }
}
