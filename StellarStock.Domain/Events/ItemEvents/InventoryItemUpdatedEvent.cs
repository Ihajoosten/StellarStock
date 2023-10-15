namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemUpdatedEvent
    {
        public InventoryItem InventoryItem { get; }

        public InventoryItemUpdatedEvent(InventoryItem item)
        {
            InventoryItem = item;
        }
    }
}
