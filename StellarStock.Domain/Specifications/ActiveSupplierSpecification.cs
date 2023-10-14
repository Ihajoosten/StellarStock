namespace StellarStock.Domain.Specifications
{
    public class ActiveSupplierSpecification : ISpecification<Supplier>
    {
        public bool IsSatisfiedBy(Supplier supplier)
        {
            return supplier.IsActive && supplier.ValidityPeriod.IsSatisfiedBy(DateTime.Now);
        }
    }

}
