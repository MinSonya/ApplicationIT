using Microsoft.EntityFrameworkCore;

namespace ApplicationIT.Models
{
    public class RequestContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Application;Username=postgres;Password=1234567");
        }

        public DbSet<Request> Requests { get; set; } = null!;
    }
}

