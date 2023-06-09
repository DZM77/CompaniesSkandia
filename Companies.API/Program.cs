using Companies.API.Data;
using Companies.API.Entities;
using Companies.API.Extensions;
using Companies.API.Middleware;
using Companies.API.Repositories;
using Companies.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Notify.Messages;
using NServiceBus;
using System.Security.Claims;

namespace Companies.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Companies.API.Sender");
                var transport = endpointConfiguration.UseTransport(new LearningTransport());
                transport.RouteToEndpoint(
                    assembly: typeof(CompanyControllerMessage).Assembly,
                    destination: "Notify.Endpoint");

                endpointConfiguration.SendOnly();

                return endpointConfiguration;
            });

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               //  options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));
               options.UseInMemoryDatabase("CompaniesInMemoryDB"));


            builder.Services.AddControllers(configure =>
            {
                     configure.ReturnHttpNotAcceptable = true; 

                     var policy = new AuthorizationPolicyBuilder()
                                           .RequireClaim(ClaimTypes.NameIdentifier)
                                           .RequireRole("Admin")
                                           .Build();

                     configure.Filters.Add(new AuthorizeFilter(policy));

            })
                            .AddNewtonsoftJson()
                            .AddXmlDataContractSerializerFormatters();

            
            //builder.Services.AddAuthorization(opt =>
            //{
            //    opt.AddPolicy("Test", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireRole("Admin");
            //        //  policy.RequireRole("Employee");
            //    });
            //});


            builder.Services.AddAuthentication();
            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 3;
                options.User.RequireUniqueEmail = true;
            })
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCorsPolicy();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            //builder.Services.AddScoped      - The request share the same instance (Creates new instance for each request)
            //builder.Services.AddTransient   - Creates a new instance each time the application needs it (possible multiple instanses in the same request)
            //builder.Services.AddSingleton   - All request shares same instance

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.ConfigureJWT(builder.Configuration);

            var app = builder.Build();


            app.ConfigureExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                await app.SeedDataAsync();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();
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

            await app.RunAsync();
        }
    }
}