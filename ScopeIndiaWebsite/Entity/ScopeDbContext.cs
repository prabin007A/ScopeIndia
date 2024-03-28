using Microsoft.EntityFrameworkCore;
using ScopeIndiaWebsite.Models;
namespace ScopeIndiaWebsite.Entity
{
    public class ScopeDbContext:DbContext
    {
        public ScopeDbContext(DbContextOptions<ScopeDbContext> options) : base(options)
        {

        }
        public DbSet<Students> Student { get; set; }
        public DbSet<LogIn> LoginDetails { get; set; }
    }
}
