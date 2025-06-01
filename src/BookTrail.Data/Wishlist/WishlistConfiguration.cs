using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTrail.Data
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(w => new { w.UserId, w.BookId });

            // Configure relationships
            builder.HasOne(w => w.Book)
                .WithMany()
                .HasForeignKey(w => w.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.User)
                .WithMany(u => u.Wishlist)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}