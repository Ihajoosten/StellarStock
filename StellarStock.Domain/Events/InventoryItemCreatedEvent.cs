using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
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
