using Bogus;
using Companies.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Companies.API.Data
{
    public class SeedData
    {
        private static ApplicationDbContext db = default!;
        private static UserManager<User> userManager;
        private static RoleManager<IdentityRole> roleManager;
        private static string passWord;
        private const string employeeRole = "Employee";
        private const string adminRole = "Admin";

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            db = context ?? throw new ArgumentNullException(nameof(context));

            if (await db.Companies.AnyAsync()) return;


            userManager = serviceProvider.GetRequiredService<UserManager<User>>() ??
           throw new ArgumentNullException(nameof(userManager));

            roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>() ??
            throw new ArgumentNullException(nameof(userManager));

            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var defaultPassWord = config["DefaultPassWord"];

            if (string.IsNullOrWhiteSpace(defaultPassWord))
            {
                throw new ArgumentException($"'{nameof(defaultPassWord)}' cannot be null or whitespace.", nameof(defaultPassWord));
            }

            passWord = defaultPassWord;

            await CreateRolesAsync(new[] { employeeRole, adminRole });

            var companies = GenerateCompanies(3);
            await db.AddRangeAsync(companies);
            await db.SaveChangesAsync();

            await GenerateEmployeesAsync(30, companies);
            await CreateAdmin();
        }

        private static async Task CreateAdmin()
        {
            var email = "kalle@anka.com";

            var kalle = new User
            {
                UserName = email,
                Email = email,
                Name = "Kalle Anka",
                Age = 50,
                Position = "Developer"
            };

            await userManager.CreateAsync(kalle, passWord);
            await AddToRoleeASync(kalle, adminRole);
        }

        private static async Task CreateRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static List<Company> GenerateCompanies(int nrOfCompanies)
        {
            var faker = new Faker<Company>("sv").Rules((f, c) =>
                 {
                     c.Name = f.Company.CompanyName();
                     c.Country = f.Address.Country();
                     c.Address = f.Address.FullAddress();
                    // c.Employees = GenerateEmployees(f.Random.Int(min: 2, max: 10));
                 });

            return faker.Generate(nrOfCompanies);
        }

        private static async Task GenerateEmployeesAsync(int nrOfEmplyees, List<Company> companies)
        {
            string[] positions = new[] { "Developer", "Tester", "Manager" };

            var faker = new Faker<User>("sv").Rules((f, e) =>
            {
                e.Name = f.Person.FullName;
                e.Age = f.Random.Int(min: 18, max: 70);
                e.Position = positions[f.Random.Int(0, positions.Length - 1)];
                e.Email = f.Person.Email;
                e.UserName = f.Person.Email;
                e.Company = companies[f.Random.Int(0, companies.Count - 1)];
            });

            var employees = faker.Generate(nrOfEmplyees);

            foreach (var employee in employees)
            {
                var result = await userManager.CreateAsync(employee, passWord);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                await AddToRoleeASync(employee, employeeRole);
            }
        }

        private static async Task AddToRoleeASync(User employee, string role)
        {
            if (await userManager.IsInRoleAsync(employee, role)) return;
            var result = await userManager.AddToRoleAsync(employee, role);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }


}
