using System.Text;
using MangaScans.Domain.Entities;
using MangaScans.Identity.Configuration;
using MangaScans.Identity.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MangaScans.Api.Extensions;

public static class AuthenticationSetup
{
    public static void AddAuthenticationSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = configuration.GetSection("JwtConfig");
        var SecurityKey = 
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TokenSecurityKey")));

        services.Configure<JWTConfig>(x =>
        {
            x.Issuer = jwtConfig["Issuer"];
            x.Audience = jwtConfig["Audience"];
            x.SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
        });

        var tokenParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfig["Issuer"],
            
            ValidateAudience = true,
            ValidAudience = jwtConfig["Audience"],
            
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKey,
            
            RequireExpirationTime = true,
            ValidateLifetime = true,
            
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        { 
            x.TokenValidationParameters = tokenParameters; 
        });
    }

    public static async Task UseAutenticationSetup(this WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>() ?? throw new ArgumentNullException(nameof(RoleManager<IdentityRole>));
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>() ?? throw new ArgumentNullException(nameof(UserManager<User>));

            string[] roles = new[]
            {
                Roles.User,
                Roles.Administrator
            };

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            
            var adminUser = new User()
            {
                UserName = Environment.GetEnvironmentVariable("Admin_Email") ?? "admin@admin.com",
                Email = Environment.GetEnvironmentVariable("Admin_Email") ?? "admin@admin.com",
                EmailConfirmed = true
            };
            
            string password = Environment.GetEnvironmentVariable("AdminPassword") ?? "Admin@123456";
            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var result = await userManager.CreateAsync(adminUser, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, Roles.Administrator);
                else
                    throw new Exception("Failed to create Admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}