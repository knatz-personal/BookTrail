using BookTrail.Seeder;
using Microsoft.Extensions.Configuration;

try
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
            true)
        .Build();

    SeederService seeder = new(configuration);
    await seeder.SeedAsync(args);
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine(ex.InnerException);
    Console.WriteLine(ex.StackTrace);
    Console.ResetColor();
    Console.ReadLine();
}