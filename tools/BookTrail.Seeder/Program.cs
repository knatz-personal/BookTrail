using Bogus;
using BookTrail.API.Data;
using BookTrail.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BookTrail.Seeder;

try
{
    // Build configuration
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");
    }

    // Bind seeder options
    var seederOptions = new SeederOptions();
    configuration.GetSection(SeederOptions.ConfigurationSection).Bind(seederOptions);

    // Create DbContext
    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    using var context = new ApplicationDbContext(optionsBuilder.Options);

    // Configure EF Core for better performance
    context.ChangeTracker.AutoDetectChangesEnabled = false;
    context.Database.SetCommandTimeout(TimeSpan.FromMinutes(seederOptions.Database.TimeoutMinutes));

    // Test database connection
    Console.WriteLine("Testing database connection...");
    try 
    {
        await context.Database.CanConnectAsync();
        Console.WriteLine("Database connection successful!");
    }
    catch (Exception ex)
    {
        throw new Exception($"Failed to connect to database. Connection string: {connectionString}", ex);
    }

    try
    {
        // Clear existing todos
        Console.WriteLine("Clearing existing todos...");
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Todos\" RESTART IDENTITY");

        // Create Faker for Todo items
        var todoOptions = seederOptions.Entities.Todo;
        var todoFaker = new Faker<Todo>()
            .RuleFor(t => t.Title, f => f.Lorem.Sentence(
                todoOptions.Title.MinWords, 
                todoOptions.Title.MaxWords).TrimEnd('.'))
            .RuleFor(t => t.DueBy, f => DateOnly.FromDateTime(f.Date.Future(todoOptions.Date.FutureDaysRange)))
            .RuleFor(t => t.IsComplete, f => f.Random.Bool(todoOptions.Status.CompletionRate));

        // Generate todos in batches
        Console.WriteLine($"Generating {todoOptions.Count:N0} todo items...");
        var processedCount = 0;

        while (processedCount < todoOptions.Count)
        {
            var remainingTodos = Math.Min(seederOptions.Performance.BatchSize, todoOptions.Count - processedCount);
            var todos = todoFaker.Generate(remainingTodos);
            
            await context.Todos.AddRangeAsync(todos);
            await context.SaveChangesAsync();
            
            // Clear the change tracker after each batch to free memory
            context.ChangeTracker.Clear();
            
            processedCount += remainingTodos;
            Console.WriteLine($"Progress: {processedCount:N0} / {todoOptions.Count:N0} todos created");
        }

        Console.WriteLine($"Successfully seeded {processedCount:N0} todo items to the database.");
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error during seeding: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
        Console.ResetColor();
        Environment.Exit(1);
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    Console.ResetColor();
    Environment.Exit(1);
}
