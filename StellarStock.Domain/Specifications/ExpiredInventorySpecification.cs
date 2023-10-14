namespace StellarStock.Domain.Specifications
{
    public class ExpiredInventorySpecification : ISpecification<InventoryItem>
    {
        public bool IsSatisfiedBy(InventoryItem item)
        {
            return item.ValidityPeriod.EndDate < DateTime.Now;
        }
    }
}