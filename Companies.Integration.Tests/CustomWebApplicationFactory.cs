using Companies.API;
using Companies.API.Data;
using Companies.API.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;

namespace Companies.Integration.Tests
{
    public class CustomWebApplicationFactory<T> : WebApplicationFactory<Program>, IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                var serviceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                var scope = services.BuildServiceProvider().CreateScope();
                
                    var provider = scope.ServiceProvider;
                    var context = provider.GetRequiredService<ApplicationDbContext>();
                    
                    //Viktigt här skulle vi behöva använda UserManager för att skapa användare!!!! Om vi vill testa authorization!!!
                    //Se kod i SeedData :)
                    context.Companies.AddRange(new Company[]
                    {
                    new Company()
                    {
                        Name = "TestCompanyName",
                        Address = "TestAdress",
                        Country = "TestCountry",
                        Employees = new User[]
                        {
                            new User
                            {
                                UserName = "TestUserName",
                                Email = "test@test.com",
                                Age = 50,
                                Name = "TestName",
                                Position = "TestPosition"
                            }
                        }
                    }
                    });

                    context.SaveChanges();
                    Context = context;
                
            });
        }

        public override ValueTask DisposeAsync()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            return base.DisposeAsync();
        }
        
    }
}