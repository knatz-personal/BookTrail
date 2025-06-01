namespace BookTrail.Data
{
    public class ReadLog
    {
        public Guid UserId { get; set; }
        public long BookId { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }
        public short Rating { get; set; }

        public EReadingStatus Status { get; set; } = EReadingStatus.ToRead;


        public Review Review { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
    }
}