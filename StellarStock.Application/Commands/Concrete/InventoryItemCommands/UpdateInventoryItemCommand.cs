namespace StellarStock.Application.Commands.Concrete.InventoryItemCommands
{
    public class UpdateInventoryItemCommand : IInventoryItemCommand
    {
        public string Id { get; set; }
        public string NewName { get; set; }
        public string NewDescription { get; set; }
        public ItemCategory NewCategory { get; set; }
        public int NewPopularityScore { get; set; }
        public ProductCodeVO NewProductCode { get; set; }
        public QuantityVO NewQuantity { get; set; }
        public MoneyVO NewMoney { get; set; }
    }
}