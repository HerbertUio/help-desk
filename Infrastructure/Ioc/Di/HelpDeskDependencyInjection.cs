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
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddApplicationServices();
        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? configuration["ConnectionStrings:remoteConnection"]
                               ?? throw new ArgumentNullException(nameof(configuration), "Connection string no encontrada ('DefaultConnection' o 'remoteConnection').");

        services.AddDbContext<HelpDeskDbContext>(options =>
            options.UseNpgsql(connectionString));
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        // Registrar otros repositorios aquí...
        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
        // Registrar otros servicios de aplicación aquí...

        // Registrar Validadores (Alternativa: hacerlo en Api/Program.cs)
        // services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>(ServiceLifetime.Scoped);

        return services;
    }
}