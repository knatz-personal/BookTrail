using System.Collections.ObjectModel;
using Shared.Extensions;

namespace BookTrail.Data
{
    public class BookShelf : EntityBase<long>
    {
        public Guid UserId { get; set; }
        public string ShelfName { get; set; } = string.Empty;
        public User User { get; set; }
        public Collection<BookShelfItem> ShelfItems { get; set; }
    }
}