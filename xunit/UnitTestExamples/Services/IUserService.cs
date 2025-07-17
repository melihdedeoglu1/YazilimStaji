using UnitTestExamples.Models;

namespace UnitTestExamples.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
    }
}
