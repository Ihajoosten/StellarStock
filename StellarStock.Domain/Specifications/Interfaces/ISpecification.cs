namespace StellarStock.Domain.Specifications.Interfaces
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
    }
}
