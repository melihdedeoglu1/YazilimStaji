using AuthService.Data;
using AuthService.DTOs;
using AuthService.Models;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly HttpClient _httpClient;

        private readonly AuthContext _context;

        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthServices(AuthContext context, HttpClient httpClient , JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _httpClient = httpClient;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Auth> RegisterAsync(RegisterDto registerDto)
        {
            var user = new Auth
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password 
            };
            _context.Auths.Add(user);
            await _context.SaveChangesAsync();


            var customer = new
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password
            };


            await _httpClient.PostAsJsonAsync("http://localhost:5249/api/customer/from-auth", customer);

            

            return user;
        }

        public async Task<Auth> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Auths
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Email);


            return user;
        }

       

        public async Task<Auth> GetUserByIdAsync(Guid id)
        {
            return await _context.Auths.FindAsync(id);
        }

        


    }
}
