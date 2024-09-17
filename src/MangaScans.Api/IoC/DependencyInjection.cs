using MangaScans.Data.Context;
using MangaScans.Data.Repositories;
using MangaScans.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangaScans.Api.IoC;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(x =>
        {
            string connectionString = configuration["ConnectionStrings:MySQLConnection"] ?? throw new ArgumentNullException("connection string not found");
            
            x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("MangaScans.Api"));
        });

        services.AddScoped<IRepositoryManga, RepositoryManga>();
    }
}