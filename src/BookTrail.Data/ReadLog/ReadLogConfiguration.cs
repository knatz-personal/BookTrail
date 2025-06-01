using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTrail.Data
{
    public class ReadLogConfiguration : IEntityTypeConfiguration<ReadLog>
    {
        public void Configure(EntityTypeBuilder<ReadLog> builder)
        {
            builder.HasKey(rl => new { rl.UserId, rl.BookId });

            // Configure relationships
            builder.HasOne(rl => rl.Book)
                .WithMany()
                .HasForeignKey(rl => rl.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rl => rl.User)
                .WithMany(u => u.ReadLogs)
                .HasForeignKey(rl => rl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}