using Nps.Domain.Entities;
namespace Nps.Application.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetAnyAdminAsync();
    Task UpdateAsync(User user);
    Task AddAsync(User user);
}
