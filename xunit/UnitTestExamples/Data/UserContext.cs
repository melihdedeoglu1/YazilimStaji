using Microsoft.EntityFrameworkCore;
using UnitTestExamples.Models;

namespace UnitTestExamples.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
    }
    
}
