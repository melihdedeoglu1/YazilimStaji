using Kullanici.API.DTOs;
using Kullanici.API.Models;
using Kullanici.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;

namespace Kullanici.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<User> Register(UserForRegisterDto userForRegisterDto)
        {
            if (await _userRepository.UserExists(userForRegisterDto.Email))
                throw new Exception("Email address already in use."); 

            CreatePasswordHash(userForRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow,
                Role = "User"
            };
            
            var createdUser = await _userRepository.Register(userToCreate);

            return createdUser;
        }


        public async Task<string> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _userRepository.GetUserByEmail(userForLoginDto.Email);

            if (userFromRepo == null)
                throw new Exception("User not found.");

            if (!VerifyPasswordHash(userForLoginDto.Password, userFromRepo.PasswordHash, userFromRepo.PasswordSalt))
                throw new Exception("Wrong password.");

            
            return _jwtTokenGenerator.GenerateToken(userFromRepo.Id,userFromRepo.Username,userFromRepo.Email,userFromRepo.Role);
        }

        public async Task<UserForDetailDto?> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return null; 
            }

           
            var userDto = _mapper.Map<UserForDetailDto>(user);
            return userDto;
        }



        public async Task<DateTime?> GetUserByIdForDateTime(int id) 
        {
            var userDatetime = await _userRepository.GetUserByIdForDateTime(id);
            if (userDatetime == null) 
            {
                return DateTime.Now;
            }

            return userDatetime;
        }




        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
