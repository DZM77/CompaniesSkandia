using Companies.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {

       public DbSet<Company> Companies => Set<Company>();
       public DbSet<Employee> Employees => Set<Employee>();

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
