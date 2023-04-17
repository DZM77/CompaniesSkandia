using Companies.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Company> Companies => Set<Company>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.Entity<Company>().HasData()
        }

    }
}
