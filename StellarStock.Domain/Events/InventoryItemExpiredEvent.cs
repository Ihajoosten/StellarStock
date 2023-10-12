using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
{
    public class InventoryItemExpiredEvent
    {
        public InventoryItem InventoryItem { get; }

        public InventoryItemExpiredEvent(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
        }
    }
}
