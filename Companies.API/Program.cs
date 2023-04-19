
using Companies.API.Data;
using Companies.API.Extensions;
using Companies.API.Middleware;
using Microsoft.EntityFrameworkCore;

namespace Companies.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));


            builder.Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
                            .AddNewtonsoftJson()
                            .AddXmlDataContractSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCorsPolicy();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            var app = builder.Build();
            await app.SeedDataAsync();




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseDemo();

            app.Map("/hej", builder =>
            {
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("Application return response here if route starts with hej");
                });
            });

            app.MapWhen(context => context.Request.Path.StartsWithSegments("/api/map"), builder => builder.Run(async context =>
            {
                await context.Response.WriteAsync("Application return response from MapWhen");
            }));


            app.MapControllers();

            app.Run();
        }
    }
}