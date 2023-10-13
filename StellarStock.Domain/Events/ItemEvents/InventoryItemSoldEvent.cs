namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemSoldEvent
    {
        public string InventoryItemId { get; }
        public int QuantitySold { get; }

        public InventoryItemSoldEvent(string inventoryItemId, int quantitySold)
        {
            InventoryItemId = inventoryItemId;
            QuantitySold = quantitySold;
        }
    }
}
