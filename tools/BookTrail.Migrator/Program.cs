using BookTrail.Migrator;

MigrationService migrationService = new();
await migrationService.RunMigratorAsync(args);