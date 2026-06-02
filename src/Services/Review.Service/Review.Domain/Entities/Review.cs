using Review.Domain.Enums;

namespace Review.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; private set; }
        public Guid TripId { get; private set; }

        // celui qui écrit
        public Guid ReviewerId { get; private set; }

        // celui qui reçoit
        public Guid RevieweeId { get; private set; }

        // Passenger ou Driver
        public ReviewerType ReviewerType { get; private set; } 

        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Review() { }

        public static Review Create(
            Guid tripId,
            Guid reviewerId,
            Guid revieweeId,
            ReviewerType reviewerType,
            int rating,
            string? comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("La note doit être entre 1 et 5.");

            return new Review
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                ReviewerId = reviewerId,
                RevieweeId = revieweeId,
                ReviewerType = reviewerType,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
