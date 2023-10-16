namespace StellarStock.Application.Commands.Concrete.InventoryItemCommands
{
    public class DecreaseInventoryItemQuantityCommand : IInventoryItemCommand
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}
