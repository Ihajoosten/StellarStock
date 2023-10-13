namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemExpiredEvent
    {
        public string InventoryItemId { get; }

        public InventoryItemExpiredEvent(string inventoryItemId)
        {
            InventoryItemId = inventoryItemId;
        }
    }
}
