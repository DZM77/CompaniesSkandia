using Companies.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Companies.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.Migrate();

                try
                {
                    await SeedData.InitAsync(db);
                }
                catch (Exception e)
                {
                    //ToDo: Log errors when seeding!
                    throw;
                }
            }

        }
    }
}
