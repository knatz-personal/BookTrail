namespace BookTrail.Seeder;

public class SeederOptions
{
    public const string ConfigurationSection = "SeederOptions";
    
    public DatabaseOptions Database { get; set; } = new();
    public PerformanceOptions Performance { get; set; } = new();
    public EntitiesOptions Entities { get; set; } = new();

    public class DatabaseOptions
    {
        public int TimeoutMinutes { get; set; } = 10;
    }

    public class PerformanceOptions
    {
        public int BatchSize { get; set; } = 50000;
    }

    public class EntitiesOptions
    {
        public TodoOptions Todo { get; set; } = new();

        public class TodoOptions
        {
            public int Count { get; set; } = 1000000;
            public TitleOptions Title { get; set; } = new();
            public DateOptions Date { get; set; } = new();
            public StatusOptions Status { get; set; } = new();

            public class TitleOptions
            {
                public int MinWords { get; set; } = 3;
                public int MaxWords { get; set; } = 2;
            }

            public class DateOptions
            {
                public int FutureDaysRange { get; set; } = 30;
            }

            public class StatusOptions
            {
                public float CompletionRate { get; set; } = 0.3f;
            }
        }
    }
}