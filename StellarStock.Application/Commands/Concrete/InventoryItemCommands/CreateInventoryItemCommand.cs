namespace StellarStock.Application.Commands.Concrete.InventoryItemCommands
{
    public class CreateInventoryItemCommand : IInventoryItemCommand
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required ItemCategory Category { get; set; }
        public required int PopularityScore { get; set; }
        public required ProductCodeVO ProductCode { get; set; }
        public required QuantityVO Quantity { get; set; }
        public required MoneyVO Money { get; set; }
        public required DateRangeVO ValidityPeriod { get; set; }
        public required string WarehouseId { get; set; }
        public required string SupplierId { get; set; }
    }
}