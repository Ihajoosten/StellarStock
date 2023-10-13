namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemRestockedEvent
    {
        public string InventoryItemId { get; }
        public int QuantityRestocked { get; }

        public InventoryItemRestockedEvent(string inventoryItemId, int quantityRestocked)
        {
            InventoryItemId = inventoryItemId;
            QuantityRestocked = quantityRestocked;
        }
    }
}
