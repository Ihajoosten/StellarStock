namespace StellarStock.Application.Commands.InventoryItemCommands
{
    public class UpdateInventoryItemCommand : ICommand
    {
        public string InventoryItemId { get; set; }
        public string NewName { get; set; }
        public string NewDescription { get; set; }
        public ItemCategory NewCategory { get; set; }
        public int NewPopularityScore { get; set; }
        public ProductCodeVO NewProductCode { get; set; }
        public QuantityVO NewQuantity { get; set; }
        public MoneyVO NewMoney { get; set; }
    }
}
