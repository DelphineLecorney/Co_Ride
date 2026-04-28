using Booking.Domain.Enums;

namespace Booking.Domain.Entities
{
    /// <summary>
    /// Entité Booking représentant une réservation de trajet.
    /// </summary>
    public class BookingEntity
    {
        public Guid Id { get; private set; }
        public Guid TripId { get; private set; }
        public Guid PassengerId { get; private set; }
        public int SeatsCount { get; private set; }
        public decimal TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }
        public string? CancellationReason { get; private set; }

        private BookingEntity() { }

        // Créer une nouvelle réservation
        public static BookingEntity Create(
            Guid id,
            Guid tripId,
            Guid passengerId,
            int seatsCount,
            decimal totalPrice)
        {
            if (seatsCount <= 0)
                throw new ArgumentException("Le nombre de sièges doit être supérieur à 0", nameof(seatsCount));

            if (totalPrice < 0)
            {
                throw new ArgumentException("Le prix ne peut pas être négatif ", nameof(totalPrice));
            }

            return new BookingEntity
            {
                Id = id,
                TripId = tripId,
                PassengerId = passengerId,
                SeatsCount = seatsCount,
                TotalPrice = totalPrice,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }
        
        // Confirmer la réservation
        public void COnfirm()
        {
            if (Status != BookingStatus.Pending)
                throw new InvalidOperationException("Impossible de confimer le status {Status}");

            Status = BookingStatus.Confirmed;
            UpdatedAt = DateTime.UtcNow;
        }

        // Annuler la réservation
        public void Cancel(string reason)
        {
            if (Status == BookingStatus.Cancelled)
                throw new InvalidOperationException("La réservation est déjà annulée");

            if (Status == BookingStatus.Completed)
                throw new InvalidOperationException("Impossible d'annuler une réservation terminée");

            Status = BookingStatus.Cancelled;
            CancellationReason = reason;
            CancelledAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Marquer la réservation comme terminée
        public void Complete()
        {
            if (Status != BookingStatus.Confirmed)
                throw new InvalidOperationException("Impossible de marquer la réservation comme complète");

            Status = BookingStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}