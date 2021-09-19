using Microsoft.EntityFrameworkCore;
using QTWithMe.Models;

namespace QTWithMe.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
    }
}