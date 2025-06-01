using System.ComponentModel.DataAnnotations;
using Shared.Extensions;

namespace BookTrail.Data
{
    public class Review : EntityBase<long>
    {
        public long BookId { get; set; }
        public Guid UserId { get; set; }

        [Range(1, 5)] public short Rating { get; set; }

        [MaxLength(1000)] public string Content { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
    }
}