using Microsoft.EntityFrameworkCore;
using WebApplication7.Model;

namespace WebApplication7.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<bilgi> bilgiler { get; set; }
    }

}
