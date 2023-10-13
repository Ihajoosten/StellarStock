namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemValidityExtendedEvent
    {
        public string InventoryItemId { get; }
        public DateTime NewValidityPeriod { get; }

        public InventoryItemValidityExtendedEvent(string inventoryItemId, DateTime newValidityPeriod)
        {
            InventoryItemId = inventoryItemId;
            NewValidityPeriod = newValidityPeriod;
        }
    }
}
