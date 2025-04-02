using Application.Services;
using Domain.Repositories;
using Infrastructure.Database.EntityFramework.Context;
using Infrastructure.Database.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Ioc.Di;

public static class HelpDeskDependencyInjection
{
    public static IServiceCollection RegisterDatabaseServices(this IServiceCollection collection, IConfiguration configuration)
    {
        string connectionString = configuration["ConnectionStrings:DefaultConnection"];
        collection.AddDbContext<HelpDeskDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            }
        );
        return collection;
    }

    public static IServiceCollection RegisterProviders(this IServiceCollection collection)
    {
        return collection;
    }
    
    public static IServiceCollection RegisterValidators (this IServiceCollection collection)
    {
        return collection;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection collection)
    {
        collection.AddTransient<UserService>();
        return collection;
    }
    
    public static IServiceCollection RegisterRepositories(this IServiceCollection collection)
    {
        collection.AddTransient<IUserRepository, UserRepository>();
        return collection;
    }
}