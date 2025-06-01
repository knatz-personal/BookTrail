namespace BookTrail.Data
{
    public class Wishlist
    {
        public Guid UserId { get; set; }
        public long BookId { get; set; }
        public DateTime DateAdded { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
    }
}