namespace Booking.Domain.Exceptions
{
    public class TripNotFoundException : Exception
    {
        public TripNotFoundException(Guid tripId)
            : base($"Le trajet avec l'ID {tripId} n'a pas été trouvé.")
        {
        }
    }
}
