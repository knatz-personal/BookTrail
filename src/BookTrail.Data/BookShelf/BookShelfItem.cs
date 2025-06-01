namespace BookTrail.Data
{
    public class BookShelfItem
    {
        public long ShelfId { get; set; }
        public long BookId { get; set; }
        public DateTime DateAdded { get; set; }
        public Book Book { get; set; }
        public BookShelf Shelf { get; set; }
    }
}