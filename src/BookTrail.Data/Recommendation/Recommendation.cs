using Shared.Extensions;

namespace BookTrail.Data
{
    public class Recommendation : EntityBase<long>
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public long BookId { get; set; }
        public Book Book { get; set; } = null!;


        public Guid RecommendedById { get; set; }
        public User RecommendedBy { get; set; }


        public string Reason { get; set; }
        public DateTime DateRecommended { get; set; } = DateTime.UtcNow;

        public ERecommendationStatus Status { get; set; } = ERecommendationStatus.New;
    }
}