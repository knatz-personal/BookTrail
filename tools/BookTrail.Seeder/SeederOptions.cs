namespace BookTrail.Seeder
{
    public class SeederOptions
    {
        public const string ConfigurationSection = "SeederOptions";

        public DatabaseOptions Database { get; set; } = new();

        public SeedQuantityOptions SeedQuantities { get; set; }
    }
}