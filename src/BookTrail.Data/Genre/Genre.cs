using Shared.Extensions;

namespace BookTrail.Data
{
    public class Genre : EntityBase<Guid>
    {
        public string Name { get; set; } = null!;

        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
}