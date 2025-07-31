using Kullanici.API.Models;

namespace Kullanici.API.Repositories
{
    public interface IUserRepository
    {
        Task<User> Register(User user);
        Task<bool> UserExists(string email);
        Task<User> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
        Task<DateTime?> GetUserByIdForDateTime(int id);

        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
    }
}
