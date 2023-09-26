using Microsoft.EntityFrameworkCore;
using course_work_server.entities;
using Microsoft.Extensions.Options;

namespace course_work_server.Controllers
{
    

    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
