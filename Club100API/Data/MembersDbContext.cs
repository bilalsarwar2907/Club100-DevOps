using Microsoft.EntityFrameworkCore;
using Club100API.Models;
namespace Club100API.Data
{
    public class MembersDbContext : DbContext
    {
        public MembersDbContext(DbContextOptions<MembersDbContext> options) : base(options)
        {
        }
        public DbSet<Member> Members { get; set; }
    }
}
