using backendMpact.Models;
using Microsoft.EntityFrameworkCore;

namespace backendMpact.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }   // Creates Users table on the database
        public DbSet<TaskItem> Tasks { get; set; }
    }
}

