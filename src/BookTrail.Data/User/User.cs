using Microsoft.AspNetCore.Identity;

namespace BookTrail.Data
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Profile information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Navigation properties
        public ICollection<BookShelf> Shelves { get; set; } = new List<BookShelf>();
        public ICollection<ReadLog> ReadLogs { get; set; } = new List<ReadLog>();
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
        public ICollection<Wishlist> Wishlist { get; set; } = new List<Wishlist>();
    }
}