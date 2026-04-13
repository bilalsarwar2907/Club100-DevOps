using Microsoft.EntityFrameworkCore;
using Club100API.Models;
namespace Club100API.Data
{
    public class MembersDbContext : DbContext
    {
        //Important naming remeber to match in program.cs
        public MembersDbContext(DbContextOptions<MembersDbContext> options) : base(options)
        {
        }
        public DbSet<Member> Members { get; set; }
    }
}
