namespace StellarStock.Domain.Specifications
{
    public class PopularProductsSpecification : ISpecification<InventoryItem>
    {
        private readonly int _popularityThreshold;

        public PopularProductsSpecification(int popularityThreshold)
        {
            _popularityThreshold = popularityThreshold;
        }

        public bool IsSatisfiedBy(InventoryItem item)
        {
            return item.PopularityScore > _popularityThreshold;
        }
    }
}
