using StellarStock.Domain.Entities;
using StellarStock.Domain.Specifications.Interfaces;

namespace StellarStock.Domain.Specifications
{
    public class LowStockSpecification : ISpecification<InventoryItem>
    {
        private readonly int _lowStockThreshold;

        public LowStockSpecification(int lowStockThreshold)
        {
            _lowStockThreshold = lowStockThreshold;
        }

        public bool IsSatisfiedBy(InventoryItem item)
        {
            return item.Quantity.Value < _lowStockThreshold;
        }
    }

}
