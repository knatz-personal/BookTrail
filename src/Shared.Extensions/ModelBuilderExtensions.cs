using Microsoft.EntityFrameworkCore;

namespace Shared.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        ///     Applies global query filters to entities
        /// </summary>
        public static void ApplyGlobalFilters(this ModelBuilder modelBuilder)
        {
            // Example of a global query filter that could be used to implement soft delete
            modelBuilder.Entity<EntityBase<int>>().HasQueryFilter(b => !b.IsDeleted);
        }

        /// <summary>
        ///     Configures value conversions for specific types
        /// </summary>
        public static void ConfigureValueConverters(this ModelBuilder modelBuilder)
        {
            // Example for DateOnly conversion if needed
            // var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            //     d => d.ToDateTime(TimeOnly.MinValue),
            //     d => DateOnly.FromDateTime(d));
            //
            // modelBuilder.Entity<Todo>()
            //     .Property(t => t.DueBy)
            //     .HasConversion(dateOnlyConverter);
        }
    }
}