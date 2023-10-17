namespace StellarStock.Application.Commands.Concrete.InventoryItemCommands
{
    public class DeleteInventoryItemCommand : IInventoryItemCommand
    {
        public string Id { get; set; }
    }
}