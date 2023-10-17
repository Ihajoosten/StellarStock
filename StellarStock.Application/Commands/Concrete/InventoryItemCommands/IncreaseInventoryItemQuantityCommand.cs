namespace StellarStock.Application.Commands.Concrete.InventoryItemCommands
{
    public class IncreaseInventoryItemQuantityCommand : IInventoryItemCommand
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}
