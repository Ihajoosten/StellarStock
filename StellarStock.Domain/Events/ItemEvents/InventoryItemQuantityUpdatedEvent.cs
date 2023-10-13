namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemQuantityUpdatedEvent
    {
        public string InventoryItemId { get; }
        public int NewQuantity { get; }

        public InventoryItemQuantityUpdatedEvent(string inventoryItemId, int newQuantity)
        {
            InventoryItemId = inventoryItemId;
            NewQuantity = newQuantity;
        }
    }
}
