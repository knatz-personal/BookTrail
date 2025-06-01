using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTrail.Data
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(b => b.Description)
                .HasMaxLength(2000);

            builder.Property(b => b.ISBN)
                .HasMaxLength(20);

            builder.Property(b => b.CoverImageUrl)
                .HasMaxLength(255);

            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}