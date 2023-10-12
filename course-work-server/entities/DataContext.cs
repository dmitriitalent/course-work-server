using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace course_work_server.Entities
{


    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        
        public DbSet<Race> Races { get; set; } = null!;
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
