using Companies.API.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Companies.API.Extensions;

public static class ServiceExtensions
{
    public static void AddCorsPolicy(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });


    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        //var jwtSettings = configuration.GetSection("JwtSettings");
        //setx SECRET "CompaniesSecretKey" /M    CMD as Admin

        var jwtConfiguration = new JwtConfigurations();
        configuration.Bind(JwtConfigurations.Section, jwtConfiguration);

        services.Configure<JwtConfigurations>(configuration.GetSection(JwtConfigurations.Section).Bind);

        var secretKey = Environment.GetEnvironmentVariable("SECRET");
        ArgumentNullException.ThrowIfNull(nameof(secretKey));

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = jwtConfiguration.ValidIssuer,
                   ValidAudience = jwtConfiguration.ValidAudience,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
               };
           });

    }




}
