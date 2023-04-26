using Companies.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

       public DbSet<Company> Companies => Set<Company>();
       public DbSet<User> Employees => Set<User>();

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
