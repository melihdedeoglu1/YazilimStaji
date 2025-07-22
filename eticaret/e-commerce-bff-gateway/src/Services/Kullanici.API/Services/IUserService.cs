using Kullanici.API.DTOs;
using Kullanici.API.Models;

namespace Kullanici.API.Services
{
    public interface IUserService
    {
        Task<User> Register(UserForRegisterDto userForRegisterDto);
        Task<string> Login(UserForLoginDto userForLoginDto);
        Task<UserForDetailDto?> GetUserById(int id);
    }
}
