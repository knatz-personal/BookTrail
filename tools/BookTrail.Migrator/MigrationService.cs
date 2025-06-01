using BookTrail.Data.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace BookTrail.Migrator
{
    public class MigrationService
    {
        public async Task RunMigratorAsync(string[] args)
        {
            AnsiConsole.Write(new FigletText("Book Trail").Centered().Color(Color.Blue));
            AnsiConsole.MarkupLine("[bold]Database Migrator[/]");
            AnsiConsole.WriteLine();

            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                        true)
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    AnsiConsole.MarkupLine("[red]Error: Connection string not found in configuration.[/]");
                    Console.ReadLine();
                    return;
                }

                DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
                optionsBuilder.UseNpgsql(connectionString,
                    x => x.MigrationsAssembly(GetType().Assembly.GetName().Name));

                using ApplicationDbContext context = new(optionsBuilder.Options);

                if (args.Length > 0)
                {
                    string command = args[0].ToLower();

                    switch (command)
                    {
                        case "script":
                            await ScriptAsync(context);
                            break;
                        case "update":
                            await MigrateAsync(context);
                            break;
                        case "list":
                            await ListAsync(context);
                            break;
                        case "help":
                            ShowHelp();
                            break;
                        default:
                            AnsiConsole.MarkupLine($"[red]Unknown command: {command}[/]");
                            ShowHelp();
                            break;
                    }

                    Console.ReadLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]No command provided. Defaulting to 'update'...[/]");
                    await MigrateAsync(context);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                Console.ReadLine();
            }
        }

        private void ShowHelp()
        {
            AnsiConsole.MarkupLine(@"
[yellow]Usage:[/]
dotnet run -- [command] [options]

[yellow]Commands:[/]
[green]update[/]       Apply pending migrations to the database (default)
[green]add[/] <name>   Create a new migration with the specified name
[green]remove[/]       Remove the last migration
[green]script[/]       Generate SQL script for all pending migrations
[green]list[/]         List all migrations and their status
[green]help[/]         Show this help message
");
        }

        private async Task ScriptAsync(ApplicationDbContext context)
        {
            await AnsiConsole.Status().StartAsync("Generating SQL script...", async _ =>
            {
                try
                {
                    string filename = $"migration_script_{DateTime.Now:yyyyMMddHHmmss}.sql";

                    List<string> allMigrations = context.Database.GetMigrations().ToList();
                    List<string> appliedMigrations = (await context.Database.GetAppliedMigrationsAsync()).ToList();

                    if (!allMigrations.Any())
                    {
                        AnsiConsole.MarkupLine("[yellow]No migrations found to script.[/]");
                        return;
                    }

                    string script = context.Database.GenerateCreateScript();
                    await File.WriteAllTextAsync(filename, script);
                    AnsiConsole.MarkupLine($"[green]SQL script generated: {filename}[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Failed to generate script: {ex.Message}[/]");
                    AnsiConsole.WriteException(ex);
                }
            });
        }

        private async Task MigrateAsync(ApplicationDbContext context)
        {
            List<string> pendingMigrations = (await context.Database.GetPendingMigrationsAsync()).ToList();

            if (!pendingMigrations.Any())
            {
                AnsiConsole.MarkupLine("[green]Database is up to date. No migrations to apply.[/]");
                return;
            }

            Table table = new Table()
                .AddColumn("Pending Migrations")
                .Border(TableBorder.Rounded);

            foreach (string migration in pendingMigrations)
            {
                table.AddRow(migration);
            }

            AnsiConsole.Write(table);

            bool confirm = AnsiConsole.Confirm("Do you want to apply these migrations?");
            if (!confirm)
            {
                AnsiConsole.MarkupLine("[yellow]Migration cancelled by user.[/]");
                return;
            }

            await AnsiConsole.Status().StartAsync("Applying migrations...", async _ =>
            {
                await context.Database.MigrateAsync();
                AnsiConsole.MarkupLine("[green]Successfully applied all migrations.[/]");
            });
        }

        private async Task ListAsync(ApplicationDbContext context)
        {
            await AnsiConsole.Status().StartAsync("Retrieving migrations...", async _ =>
            {
                try
                {
                    List<string> appliedMigrations = (await context.Database.GetAppliedMigrationsAsync()).ToList();
                    List<string> allMigrations = context.Database.GetMigrations().ToList();
                    List<string> pendingMigrations = allMigrations.Except(appliedMigrations).ToList();

                    if (!allMigrations.Any())
                    {
                        AnsiConsole.MarkupLine("[yellow]No migrations found in the project.[/]");
                        return;
                    }

                    Table table = new Table()
                        .AddColumn("Migration")
                        .AddColumn("Status")
                        .Border(TableBorder.Rounded);

                    foreach (string migration in allMigrations)
                    {
                        table.AddRow(migration,
                            appliedMigrations.Contains(migration) ? "[green]Applied[/]" : "[yellow]Pending[/]");
                    }

                    AnsiConsole.Write(table);

                    AnsiConsole.MarkupLine(
                        $"[blue]Total: {allMigrations.Count} migrations ({appliedMigrations.Count} applied, {pendingMigrations.Count} pending)[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Failed to list migrations: {ex.Message}[/]");
                }
            });
        }
    }
}