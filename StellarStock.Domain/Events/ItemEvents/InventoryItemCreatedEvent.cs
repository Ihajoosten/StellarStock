namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemCreatedEvent
    {
        public InventoryItem InventoryItem { get; }

        public InventoryItemCreatedEvent(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
        }
    }
}
