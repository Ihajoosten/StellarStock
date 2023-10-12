using StellarStock.Domain.Entities;

namespace StellarStock.Domain.Events
{
    public class InventoryItemDepletedEvent
    {
        public InventoryItem InventoryItem { get; }

        public InventoryItemDepletedEvent(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
        }
    }
}
