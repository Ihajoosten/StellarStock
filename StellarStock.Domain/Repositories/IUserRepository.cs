using StellarStock.Domain.Entities;
using StellarStock.Domain.Repositories.Base;

namespace StellarStock.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<AppUser> GetUserByUsernameAsync(string username);
    }
}