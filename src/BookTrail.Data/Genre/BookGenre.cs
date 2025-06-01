namespace BookTrail.Data
{
    public class BookGenre
    {
        public long BookId { get; set; }
        public Book Book { get; set; } = null!;

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
    }
}