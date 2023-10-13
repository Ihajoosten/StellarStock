namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemPriceUpdatedEvent
    {
        public string InventoryItemId { get; }
        public decimal NewPrice { get; }

        public InventoryItemPriceUpdatedEvent(string inventoryItemId, decimal newPrice)
        {
            InventoryItemId = inventoryItemId;
            NewPrice = newPrice;
        }
    }
}
