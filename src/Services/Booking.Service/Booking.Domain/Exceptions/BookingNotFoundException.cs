namespace Booking.Domain.Exceptions
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(Guid bookingId)
            : base($"La réservation avec l'ID {bookingId} n'a pas été trouvé.")
        {
        }
    }
}
