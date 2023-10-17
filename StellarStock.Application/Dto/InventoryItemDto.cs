namespace StellarStock.Application.Dto
{
    public class InventoryItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemCategory Category { get; set; }
        public int PopularityScore { get; set; }
        public ProductCodeVO ProductCode { get; set; }
        public QuantityVO Quantity { get; set; }
        public MoneyVO Money { get; set; }
        public DateRangeVO ValidityPeriod { get; set; }
        public string WarehouseId { get; set; }
        public string SupplierId { get; set; }

    }
}
