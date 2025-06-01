using BookTrail.Data.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookTrail.Migrator
{
    public class MigrationFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Adjust if needed for tooling
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    true)
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
            optionsBuilder.UseNpgsql(connectionString, x => x.MigrationsAssembly(GetType().Assembly.GetName().Name));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}