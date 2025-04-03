using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database.EntityFramework.Context;

public class HelpDeskDbContextFactory : IDesignTimeDbContextFactory<HelpDeskDbContext>
{
    public HelpDeskDbContext CreateDbContext(string[] args)
    {
        string basePath = Directory.GetCurrentDirectory(); 
        
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json"
                , optional: true, reloadOnChange: true)
            .AddUserSecrets<HelpDeskDbContextFactory>(optional: true)
            .Build();
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
             connectionString = configuration["ConnectionStrings:remoteConnection"];
             if (string.IsNullOrEmpty(connectionString))
             {
                 throw new InvalidOperationException("Connection string 'DefaultConnection' (o 'remoteConnection') no encontrada. Asegúrate que esté en appsettings.json o User Secrets.");
             }
             else
             {
                  Console.WriteLine("Advertencia: Usando connection string 'remoteConnection'. Se recomienda usar 'DefaultConnection'.");
             }
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<HelpDeskDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new HelpDeskDbContext(optionsBuilder.Options);
    }
}
