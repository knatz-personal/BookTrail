using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTrail.Data
{
    public class BookShelfItemConfiguration : IEntityTypeConfiguration<BookShelfItem>
    {
        public void Configure(EntityTypeBuilder<BookShelfItem> builder)
        {
            builder.HasKey(bsi => new { bsi.ShelfId, bsi.BookId });

            // Configure relationships
            builder.HasOne(bsi => bsi.Shelf)
                .WithMany(bs => bs.ShelfItems)
                .HasForeignKey(bsi => bsi.ShelfId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bsi => bsi.Book)
                .WithMany()
                .HasForeignKey(bsi => bsi.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}