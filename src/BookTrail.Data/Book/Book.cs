using Shared.Extensions;

namespace BookTrail.Data
{
    public class Book : EntityBase<long>
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public DateOnly? PublishedDate { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public int PageCount { get; set; }
        public string Language { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
}