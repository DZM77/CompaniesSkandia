using Bogus;
using Companies.API.Data;
using Companies.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Companies.Repositories.Data
{
    public class SeedData
    {
        private static ApplicationDbContext db = default!;

        public static async Task InitAsync(ApplicationDbContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(context));

            if (await db.Companies.AnyAsync()) return;

            //var faker = new Faker("sv");

            //List<Company> companies = new();

            //for (int i = 0; i < 50; i++)
            //{
            //    companies.Add(new Company
            //    {
            //        Name = faker.Company.CompanyName(),
            //        Country = faker.Address.Country(),
            //         Address = faker.Address.FullAddress(),
            //    });
            //}

            var companies = GenerateCompanies(3);
            await db.AddRangeAsync(companies);
            await db.SaveChangesAsync();
        }

        private static IEnumerable<Company> GenerateCompanies(int nrOfCompanies)
        {
            var faker = new Faker<Company>("sv").Rules((f, c) =>
                 {
                     c.Name = f.Company.CompanyName();
                     c.Country = f.Address.Country();
                     c.Address = f.Address.FullAddress();
                     c.Employees = GenerateEmployees(f.Random.Int(min: 2, max: 10));
                 });

            return faker.Generate(nrOfCompanies);
        }

        private static ICollection<Employee> GenerateEmployees(int nrOfEmplyees)
        {
            string[] positions = new[] { "Developer", "Tester", "Manager" };

            var faker = new Faker<Employee>("sv").Rules((f, e) =>
            {
                e.Name = f.Person.FullName;
                e.Age = f.Random.Int(min: 18, max: 70);
                e.Position = positions[f.Random.Int(0, positions.Length - 1)];
            });

            return faker.Generate(nrOfEmplyees);
        }
    }


}
