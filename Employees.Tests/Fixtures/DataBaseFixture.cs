using AutoMapper;
using Companies.API;
using Companies.API.Data;
using Companies.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Tests.Fixtures
{
    public class DataBaseFixture : IDisposable
    {
        public Mapper Mapper { get; }
        public ApplicationDbContext Context { get; }

        public DataBaseFixture()
        {
            Mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            }));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TestDataBase;Trusted_Connection=True;MultipleActiveResultSets=true")
               .Options;

            var context = new ApplicationDbContext(options);

            context.Database.Migrate();

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

        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
