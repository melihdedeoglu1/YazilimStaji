
using Kullanici.API.Data;
using Kullanici.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Kullanici.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KullaniciContext _context;

        public UserRepository(KullaniciContext context)
        {
            _context = context;
        }

        public async Task<User> Register(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;

        }

        public async Task<User> GetUserByEmail(string email)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return user;
        }



    }
}
