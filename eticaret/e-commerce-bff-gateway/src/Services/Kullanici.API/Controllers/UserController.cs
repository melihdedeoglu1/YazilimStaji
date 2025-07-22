using Kullanici.API.DTOs;
using Kullanici.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kullanici.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

            var createdUser = await _userService.Register(userForRegisterDto);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var token = await _userService.Login(userForLoginDto);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            // Token'dan kullanıcı ID'sini alıyoruz (bu kısım zaten vardı ve doğruydu)
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Token does not contain a valid user ID.");
            }

            var userId = int.Parse(userIdString);

            // Yeni servis metodumuzu çağırıyoruz
            var userDto = await _userService.GetUserById(userId);

            if (userDto == null)
            {
                return NotFound($"User with ID {userId} not found."); // Veritabanında kullanıcı bulunamazsa
            }

            return Ok(userDto);
        }

    }
}
