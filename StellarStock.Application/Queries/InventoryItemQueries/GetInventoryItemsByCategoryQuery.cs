using StellarStock.Domain.Entities;

namespace StellarStock.Application.Queries.InventoryItemQueries
{
    public class GetInventoryItemsByCategoryQuery
    {
        public ItemCategory Category { get; set; }
    }
}