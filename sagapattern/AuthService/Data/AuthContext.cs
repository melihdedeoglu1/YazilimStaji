using Microsoft.EntityFrameworkCore;
using AuthService.Models; 

namespace AuthService.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<Auth> Auths { get; set; } // Assuming Auth is a model class representing the user entity
        
    }
    
    
}
