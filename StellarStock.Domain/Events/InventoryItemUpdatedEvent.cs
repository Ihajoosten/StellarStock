using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
{
    public class InventoryItemUpdatedEvent
    {
        public InventoryItem InventoryItem { get; }

        public InventoryItemUpdatedEvent(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
        }
    }
}
