using Microsoft.EntityFrameworkCore;

namespace Club100API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Define DbSet properties for your entities here, for example:
        public DbSet<Member> Members { get; set; }
    }
}