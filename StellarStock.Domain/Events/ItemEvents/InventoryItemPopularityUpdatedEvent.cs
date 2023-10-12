namespace StellarStock.Domain.Events.ItemEvents
{
    public class InventoryItemPopularityUpdatedEvent
    {
        public string InventoryItemId { get; }
        public int NewPopularityScore { get; }

        public InventoryItemPopularityUpdatedEvent(string inventoryItemId, int newPopularityScore)
        {
            InventoryItemId = inventoryItemId;
            NewPopularityScore = newPopularityScore;
        }
    }
}
