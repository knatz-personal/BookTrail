using BookTrail.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

var config = builder.Build();

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

using var context = new ApplicationDbContext(optionsBuilder.Options);

try
{
    Console.WriteLine("Applying database migrations...");
    await context.Database.MigrateAsync();
    Console.WriteLine("Migrations applied successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error applying migrations: {ex.Message}");
    throw;
}
