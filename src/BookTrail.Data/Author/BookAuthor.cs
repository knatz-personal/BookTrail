namespace BookTrail.Data
{
    public class BookAuthor
    {
        public long BookId { get; set; }
        public Book Book { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}