using Shared.Extensions;

namespace BookTrail.Data
{
    public class Author : EntityBase<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Bio { get; set; }
        public DateOnly BirthDate { get; set; }

        public string ProfileImage { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}