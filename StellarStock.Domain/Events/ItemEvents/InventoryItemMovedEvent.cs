namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemMovedEvent
    {
        public string InventoryItemId { get; }
        public string NewLocationId { get; }

        public InventoryItemMovedEvent(string inventoryItemId, string newLocationId)
        {
            InventoryItemId = inventoryItemId;
            NewLocationId = newLocationId;
        }
    }
}
