using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories.Base;

namespace StellarStock.Domain.Repositories
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        Task<IEnumerable<Location>> GetLocationsByCityAsync(string cityId);
    }
}
