namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemRemovedEvent
    {
        public string InventoryItemId { get; }

        public InventoryItemRemovedEvent(string inventoryItemId)
        {
            InventoryItemId = inventoryItemId;
        }
    }
}
