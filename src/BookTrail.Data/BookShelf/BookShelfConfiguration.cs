using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTrail.Data
{
    public class BookShelfConfiguration : IEntityTypeConfiguration<BookShelf>
    {
        public void Configure(EntityTypeBuilder<BookShelf> builder)
        {
            builder.HasKey(bs => bs.Id);

            builder.Property(bs => bs.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Configure relationship with User
            builder.HasOne(bs => bs.User)
                .WithMany(u => u.Shelves)
                .HasForeignKey(bs => bs.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}