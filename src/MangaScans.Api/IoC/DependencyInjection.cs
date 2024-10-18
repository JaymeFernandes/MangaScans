using MangaScans.Application.Services;
using MangaScans.Data.Context;
using MangaScans.Data.Repositories;
using MangaScans.Data.Repositories.Shared;
using MangaScans.Domain.Entities;
using MangaScans.Domain.Interfaces;
using MangaScans.Identity.Context;
using MangaScans.Identity.Interfaces;
using MangaScans.Identity.Repositories;
using MangaScans.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Api.IoC;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(x =>
        {
            string connectionString = configuration["ConnectionStrings:MySQLConnection"] ?? throw new ArgumentNullException("connection string not found");
            
            x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        services.AddDbContext<AppIdentityDbContext>(x =>
        {
            string connectionString = configuration["ConnectionStrings:IdentityConnection"] ?? throw new ArgumentNullException("connection string not found");
            
            x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        
        services.AddDefaultIdentity<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityServices, IdentityServices>();
        services.AddScoped<IRepositoryCover, RepositoryCover>();
        services.AddScoped<IRepositoryManga, RepositoryManga>();
        services.AddScoped<IRepositoryChapter, RepositoryChapter>();
        services.AddScoped<IRepositoryImages, RepositoryImages>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}