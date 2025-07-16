using AuthService.DTOs;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
namespace AuthService.Services
{
    public interface IAuthServices
    {
        Task<Auth> RegisterAsync(RegisterDto registerDto);
        Task<Auth> LoginAsync(LoginDto loginDto);
        Task<Auth> GetUserByIdAsync(Guid id);
        

    }
}
